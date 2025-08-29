using PayDayExplosion.Application.Dtos;
using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Services.Panama
{
    public class FactorPAService : IFactorPAService
    {
        public List<SubspanPADto> DetermineSpanTypeWorkshiftTypeFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories, List<SpanTypeDto> spanTypes,  List<WorkshiftTypeDto> workshiftTypes, bool strictShiftRules, string payTypeCode, string employeeTypeCode, bool isSuntracsRuleContract)
        {
            // La optimización con diccionarios sigue siendo la mejor práctica.
            var payCategoriesDict = payCategories.ToDictionary(pc => pc.Id);
            var workshiftTypesDict = workshiftTypes.ToDictionary(wt => wt.Id);
            var spanTypesDict = spanTypes.ToDictionary(st => st.Id);
            var saturdayFactor = 0m;

            foreach (var subspan in subspans)
            {
                // Buscamos las entidades correspondientes de forma segura.
                if (payCategoriesDict.TryGetValue(subspan.PayCategoryId, out var payCategory) &&
                    workshiftTypesDict.TryGetValue(subspan.WorkshiftTypeId, out var workshiftType) &&
                    spanTypesDict.TryGetValue(subspan.SpanTypeId, out var spanType))
                {
                    // --- 1. CÁLCULO INICIAL DEL FACTOR ---
                    if (payCategory.Code == "RegularShift")
                    {
                        // Si es regular, multiplicamos los factores regulares de WorkshiftType y SpanType.
                        subspan.Factor1 = workshiftType.RegularFactor * spanType.RegularFactor;
                    }
                    else if (payCategory.Code == "OvertimeShift")
                    {
                        if (isSuntracsRuleContract && employeeTypeCode == "Construction" && subspan.SubTimeIn.Value.DayOfWeek == DayOfWeek.Saturday && workshiftType.Code == "Day")
                        {
                            saturdayFactor = 0.25m;
                        }
                        else 
                        {
                            saturdayFactor = 0m;
                        }

                            // Si es sobretiempo, multiplicamos los factores de sobretiempo de WorkshiftType y SpanType.
                            subspan.Factor1 = (workshiftType.OvertimeFactor + saturdayFactor) * spanType.OvertimeFactor;
                    }

                    // --- 2. NUEVA LÓGICA DE AJUSTE POR TARDANZA ---
                    // Verificamos si la regla estricta está activa y si el tipo es "LateArrival".
                    if (strictShiftRules && spanType.Code == "LateArrival")
                    {
                        if (payCategory.Code == "RegularShift")
                        {
                            // Para tardanzas en turnos regulares, se multiplica por -1.
                            subspan.Factor1 *= -1;
                        }
                        else if (payCategory.Code == "OvertimeShift")
                        {
                            // Las tardanzas en sobretiempo no se pagan, así que el factor es 0.
                            subspan.Factor1 *= 0;
                        }
                    }

                    // --- 3. CONSTRUCCIÓN DEL DISPLAYTEXT ---
                    string spanNamePart;

                    // Manejo especial para el nombre de la tardanza
                    if (spanType.Code == "LateArrival")
                    {
                        // Si el factor final es mayor a 0, se paga; de lo contrario, no.
                        spanNamePart = subspan.Factor1 > 0 ? "Tardanza Pagada" : "Tardanza NO Pagada";
                    }
                    else
                    {
                        // Para todos los demás, usamos el nombre normal.
                        spanNamePart = spanType.Name;
                    }

                    //si es pactado por hora, y el factor debe restarle, lo que hace es que sea cero para que no le sume, ya que él gana solo las horas trabajadas
                    if (subspan.Factor1 < 0 && payTypeCode == "Hourly")
                    {
                        subspan.Factor1 = 0;
                    }

                    // Concatenamos todo usando interpolación de strings.
                    // Usamos ToString("F6") para formatear el factor y que se vea consistente.
                    
                    if (isSuntracsRuleContract && payCategory.Code == "OvertimeShift" && employeeTypeCode == "Construction" && subspan.SubTimeIn.Value.DayOfWeek == DayOfWeek.Saturday && workshiftType.Code == "Day")
                    {
                        subspan.DisplayText = $"{spanNamePart} - {payCategory.Name} {workshiftType.Name} - Sábado Regla de Contrato Suntracs (x{subspan.Factor1.ToString("0.0#####")})";
                    }
                    else 
                    {
                        subspan.DisplayText = $"{spanNamePart} - {payCategory.Name} {workshiftType.Name} (x{subspan.Factor1.ToString("0.0#####")})";
                    }
                }
            }

            return subspans;
        }






        public List<SubspanPADto> DetermineWorkdayTypeFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories, WorkdayDto workday, List<WorkdayTypeDto> workdayTypes)
        {
            // La optimización con diccionarios sigue siendo la mejor práctica.
            var payCategoriesDict = payCategories.ToDictionary(pc => pc.Id);
            var workdayTypesDict = workdayTypes.ToDictionary(wt => wt.Id);

            foreach (var subspan in subspans)
            {
                // --- 1. DETERMINAR QUÉ WORKDAYTYPE USAR ---

                // Primero, validamos que el subspan tenga una fecha de entrada. Si no, lo saltamos.
                if (!subspan.SubTimeIn.HasValue)
                {
                    continue;
                }

                int workdayTypeIdToUse;
                var subspanDate = subspan.SubTimeIn.Value.Date;

                if (subspanDate == workday.Date)
                {
                    // La fecha del tramo coincide con la fecha del día de trabajo.
                    workdayTypeIdToUse = workday.WorkdayTypeId;
                }
                else if (subspanDate == workday.Date.AddDays(1) && workday.NextWorkdayTypeId.HasValue)
                {
                    // Si ambas condiciones son ciertas, usamos el valor.
                    workdayTypeIdToUse = workday.NextWorkdayTypeId.Value;
                }
                else
                {
                    // La fecha no corresponde, es un error o un caso no manejado. Saltamos este subspan.
                    // Aquí podrías agregar un log de error si lo necesitas.
                    continue;
                }

                // Buscamos las entidades correspondientes de forma segura. Si alguna no existe, saltamos.
                if (payCategoriesDict.TryGetValue(subspan.PayCategoryId, out var payCategory) &&
                    workdayTypesDict.TryGetValue(workdayTypeIdToUse, out var workdayType) )
                {
                    // --- 2. CÁLCULO DE FACTOR2 ---

                    var workdayName = "";

                    if (payCategory.Code == "RegularShift")
                    {
                        // Fórmula para turnos regulares.
                        subspan.Factor2 = (workdayType.Factor - 1) * subspan.Factor1;
                        workdayName = workdayType.Name;
                    }
                    else if (payCategory.Code == "OvertimeShift")
                    {
                        // Lógica para turnos de sobretiempo.
                        if (workdayType.Code == "Compensatory" || workdayType.Code == "CompensatoryWithSurcharge")
                        {
                            // Caso especial para compensatorios.
                            subspan.Factor2 = workdayType.Factor - 0.5m; // Usamos 'm' para indicar que es un decimal.
                            if (workdayType.Code == "Compensatory")
                            {
                                workdayName = workdayTypes.Where(wdt => wdt.Code == "NormalDay").FirstOrDefault().Name;
                            }
                            else 
                            {
                                workdayName = workdayTypes.Where(wdt => wdt.Code == "MandatoryRest").FirstOrDefault().Name;
                            }
                        }
                        else
                        {
                            // Resto de casos de sobretiempo.
                            subspan.Factor2 = workdayType.Factor;
                            workdayName = workdayType.Name;
                        }
                    }

                    // --- 3. ACTUALIZACIÓN DEL DISPLAYTEXT ---

                    // Creamos la nueva parte del texto que vamos a añadir.
                    string factorPrefix = payCategory.Code == "RegularShift" ? "+" : "x";

                    // Creamos la nueva parte del texto usando el prefijo dinámico.
                    string newDisplayTextPart = $"{workdayName} - {payCategory.Name} ({factorPrefix}{subspan.Factor2.ToString("0.0#####")})";

                    // Concatenamos al DisplayText existente.
                    subspan.DisplayText = $"{subspan.DisplayText} | {newDisplayTextPart}";
                }
            }

            return subspans;
        }





        public List<SubspanPADto> DetermineSpanDetailTypeFactor(List<SubspanPADto> subspans, List<SpanDetailTypeDto> spanDetailTypes) 
        {
            var spanDetailTypesDict = spanDetailTypes.ToDictionary(pc => pc.Id);

            foreach (var subspan in subspans)
            {
                if (spanDetailTypesDict.TryGetValue(subspan.SpanDetailTypeId, out var spanDetailType))
                {
                    subspan.Factor3 = spanDetailType.Factor;

                    if (spanDetailType.Code != "Normal")
                    { 
                        subspan.DisplayText = $"{subspan.DisplayText} | {spanDetailType.Name} (x{subspan.Factor3.ToString("0.0#####")})";
                    }
                }
            }

            return subspans;
        }



        public List<SubspanPADto> DetermineSubspanDetailTypeFactor(List<SubspanPADto> subspans, List<SubspanDetailTypeDto> subspanDetailTypes)
        {
            // Buscamos el tipo de detalle de "Exceso" UNA SOLA VEZ, antes del bucle.
            // (Asumo que tiene un código único como "OvertimeExcess" o similar para identificarlo).
            var excessDetailType = subspanDetailTypes.FirstOrDefault(sdt => sdt.Code == "OvertimeExcess");

            // Si no existe una regla de exceso, no hacemos nada.
            if (excessDetailType == null)
            {
                // Asignamos 1 a Factor4 para no afectar cálculos posteriores.
                subspans.ForEach(s => s.Factor4 = 1);
                return subspans;
            }

            foreach (var subspan in subspans)
            {
                if (subspan.OvertimeExcess)
                {
                    // Si el subspan tiene la marca de exceso, aplicamos el factor y el texto.
                    subspan.Factor4 = excessDetailType.Factor;
                    // Usamos .Trim() para quitar espacios si el DisplayText está vacío al inicio.
                    subspan.DisplayText = $"{subspan.DisplayText} | {excessDetailType.Name} (x{subspan.Factor4.ToString("0.0#####")})".Trim(' ', '|');
                }
                else
                {
                    // Si NO es OvertimeExcess, el factor es 1.
                    subspan.Factor4 = 1;
                }
            }

            return subspans;
        }



        public List<SubspanPADto> CalculateFinalFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories)
        {
            var payCategoriesDict = payCategories.ToDictionary(pc => pc.Id);

            foreach (var subspan in subspans)
            {
                // El primer 'if' se mantiene como pediste. Si Factor1 es 0, el resultado es 0.
                if (subspan.Factor1 == 0)
                {
                    subspan.Factor = 0;
                    continue; // Pasa al siguiente subspan
                }

                // --- INICIO DE LA LÓGICA MODIFICADA ---

                // Intentamos encontrar la categoría de pago del subspan
                if (payCategoriesDict.TryGetValue(subspan.PayCategoryId, out var payCategory))
                {
                    // Usamos un 'switch' para decidir qué fórmula aplicar basado en el Code
                    switch (payCategory.Code)
                    {
                        case "OvertimeShift":
                            // Fórmula para sobretiempo
                            subspan.Factor = (subspan.Factor1 * subspan.Factor3 * subspan.Factor4) * subspan.Factor2;
                            break;

                        case "RegularShift":
                            // Fórmula para turno regular
                            subspan.Factor = (subspan.Factor1 * subspan.Factor3 * subspan.Factor4) + subspan.Factor2;
                            break;
                    }
                }
            }

            return subspans;
        }




    }
}
