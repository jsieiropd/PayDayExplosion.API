using PayDayExplosion.Application.Dtos;
using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Domain.Data.Entities;
using PayDayExplosion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Services.Panama
{
    public class SubspanPAService : ISubspanPAService
    {
        public List<SubspanWithSpanDetailTypesPADto> SplitSpansByShift(List<SpanPADto> spans, ShiftDto shift, List<SpanTypeDto> spanTypes, List<PayCategoryDto> payCategories, string countryCode, bool onlyCaptureOvertime)
        {
            var subspans = new List<SubspanWithSpanDetailTypesPADto>();

            if (spans == null || !spans.Any())
            {
                return subspans;
            }

            var regularShiftId = payCategories.FirstOrDefault(pc => pc.Code == "RegularShift")?.Id ?? 0;
            var overtimeShiftId = payCategories.FirstOrDefault(pc => pc.Code == "OvertimeShift")?.Id ?? 0;

            if (regularShiftId == 0 || overtimeShiftId == 0)
            {
                throw new InvalidOperationException("Las categorías de pago base 'RegularShift' o 'OvertimeShift' no fueron encontradas.");
            }

            // 1. Obtenemos los límites del turno directamente del objeto con fechas.
            var shiftStart = shift.TimeIn;
            var shiftEnd = shift.TimeOut;

            foreach (var span in spans)
            {
                var spanTypeId = spanTypes.FirstOrDefault(wtt => wtt.Id == span.SpanTypeId)?.Id ?? 1;

                // 2. Crear una lista de "puntos de corte".
                var cutPoints = new List<DateTime> { span.TimeIn, span.TimeOut };

                // Añadimos los límites del turno solo si caen DENTRO del span.
                // La comparación ahora es entre DateTimes absolutos, lo que es correcto.
                if (shiftStart > span.TimeIn && shiftStart < span.TimeOut)
                {
                    cutPoints.Add(shiftStart);
                }
                if (shiftEnd > span.TimeIn && shiftEnd < span.TimeOut)
                {
                    cutPoints.Add(shiftEnd);
                }

                // 3. Ordenar y eliminar duplicados.
                var sortedPoints = cutPoints.Distinct().OrderBy(p => p).ToList();

                // 4. Crear los nuevos subspans a partir de los puntos de corte.
                for (int i = 0; i < sortedPoints.Count - 1; i++)
                {
                    var subspanStart = sortedPoints[i];
                    var subspanEnd = sortedPoints[i + 1];

                    if (subspanStart == subspanEnd) continue;

                    int payCategoryId;

                    if (onlyCaptureOvertime)
                    {
                        // Si el flag es true, forzamos todos los subspans a ser de sobretiempo.
                        payCategoryId = overtimeShiftId;
                    }
                    else
                    { 
                        // Determinar si el subspan está dentro o fuera del horario programado.
                        // Esta lógica ahora funciona perfectamente.
                        bool isRegularShift = subspanStart >= shiftStart && subspanEnd <= shiftEnd;
                        payCategoryId = isRegularShift ? regularShiftId : overtimeShiftId;
                    }
                        

                    subspans.Add(CreateSubspanWithSpanDetailTypesFromSpan(span, subspanStart, subspanEnd, payCategoryId, spanTypeId, countryCode));
                }
            }

            return subspans;
        }






        // Se añade el nuevo parámetro 'shift'
        public List<SubspanWithSpanDetailTypesPADto> FilterSpansByAuthorization(List<SubspanWithSpanDetailTypesPADto> subspans, ShiftDto shift, bool authOvertimePreShift, bool authOvertimePostShift, bool authOvertimeDuringMeal, List<SpanTypeDto> spanTypes, List<PayCategoryDto> payCategories)
        {
            // Si la lista está vacía, no hay nada que hacer.
            if (subspans == null || !subspans.Any() || shift == null)
            {
                return subspans;
            }

            // Estos Ids siguen siendo necesarios para la lógica de la comida.
            var regularShiftId = payCategories.FirstOrDefault(pc => pc.Code == "RegularShift")?.Id ?? 0;
            var overtimeShiftId = payCategories.FirstOrDefault(pc => pc.Code == "OvertimeShift")?.Id ?? 0;
            int? breakTimeId = spanTypes.FirstOrDefault(st => st.Code == "Break")?.Id;

            var filteredList = subspans;

            // 1. Lógica para AuthOvertimePreShift (Refactorizada)
            if (!authOvertimePreShift)
            {
                // Se eliminan todos los subspans cuya hora de inicio sea ANTERIOR a la hora de inicio del turno.
                // Se usa LINQ .Where() para una lógica más clara y directa.
                filteredList = filteredList.Where(s => s.SubTimeIn >= shift.TimeIn).ToList();
            }

            // 2. Lógica para AuthOvertimePostShift (Refactorizada)
            if (!authOvertimePostShift)
            {
                // Se eliminan todos los subspans cuya hora de fin sea POSTERIOR a la hora de fin del turno.
                filteredList = filteredList.Where(s => s.SubTimeOut <= shift.TimeOut).ToList();
            }

            // 3. Lógica para AuthOvertimeDuringMeal (Sin cambios)
            // Esta lógica sigue siendo válida, ya que opera sobre los tramos que quedaron después del filtro.
            if (authOvertimeDuringMeal && breakTimeId.HasValue)
            {
                foreach (var subspan in filteredList)
                {
                    if (subspan.PayCategoryId == regularShiftId && subspan.SpanTypeId == breakTimeId.Value)
                    {
                        subspan.PayCategoryId = overtimeShiftId;
                    }
                }
            }

            return filteredList;
        }







        public List<SubspanWithSpanDetailTypesPADto> SplitSpansAtKeyTimes(List<SubspanWithSpanDetailTypesPADto> subspans)
        {
            // Si no hay nada que procesar, retornamos una lista vacía.
            if (subspans == null || !subspans.Any())
            {
                return new List<SubspanWithSpanDetailTypesPADto>();
            }

            var finalSubspans = new List<SubspanWithSpanDetailTypesPADto>();
            var keyTimes = new TimeSpan[] {
                TimeSpan.FromHours(0),  // 12:00 AM (Cambio de día)
                TimeSpan.FromHours(6),  // 06:00 AM (Cambio de horario)
                TimeSpan.FromHours(18)  // 06:00 PM (Cambio de horario)
            };

            foreach (var subspan in subspans)
            {
                // Aseguramos que los tiempos de entrada y salida no sean nulos para trabajar con ellos.
                if (!subspan.SubTimeIn.HasValue || !subspan.SubTimeOut.HasValue) continue;

                var spanStart = subspan.SubTimeIn.Value;
                var spanEnd = subspan.SubTimeOut.Value;

                // 1. Crear la lista de "puntos de corte" inicial con el inicio y fin del subspan.
                var cutPoints = new List<DateTime> { spanStart, spanEnd };

                // 2. Iterar día por día dentro del rango del subspan para encontrar los puntos de corte.
                // Esto maneja correctamente los subspans que cruzan la medianoche.
                for (var day = spanStart.Date; day <= spanEnd.Date; day = day.AddDays(1))
                {
                    foreach (var keyTime in keyTimes)
                    {
                        var potentialCutPoint = day.Add(keyTime);

                        // Añadimos el punto de corte solo si está ESTRICTAMENTE DENTRO del subspan.
                        if (potentialCutPoint > spanStart && potentialCutPoint < spanEnd)
                        {
                            cutPoints.Add(potentialCutPoint);
                        }
                    }
                }

                // 3. Ordenar puntos y eliminar duplicados.
                var sortedPoints = cutPoints.Distinct().OrderBy(p => p).ToList();

                // 4. Crear los nuevos subspans a partir de los puntos de corte.
                for (int i = 0; i < sortedPoints.Count - 1; i++)
                {
                    var newStart = sortedPoints[i];
                    var newEnd = sortedPoints[i + 1];

                    // Evitar crear subspans de duración cero.
                    if (newStart == newEnd) continue;

                    // método de ayuda para copiar y luego modificar
                    var newSubspan = CreateCopy(subspan);
                    newSubspan.SubTimeIn = newStart;
                    newSubspan.SubTimeOut = newEnd;
                    newSubspan.ClockedHours = (decimal)(newEnd - newStart).TotalHours;
                    finalSubspans.Add(newSubspan);
                }
            }

            return finalSubspans;
        }



        public List<SubspanWithSpanDetailTypesPADto> ClassifySpansByWorkshiftType(List<SubspanWithSpanDetailTypesPADto> subspans, List<WorkshiftTypeDto> workshiftTypes, List<PayCategoryDto> payCategories)
        {
            if (subspans == null || !subspans.Any())
            {
                return subspans;
            }

            var regularShiftId = payCategories.FirstOrDefault(pc => pc.Code == "RegularShift")?.Id ?? 0;
            var overtimeShiftId = payCategories.FirstOrDefault(pc => pc.Code == "OvertimeShift")?.Id ?? 0;

            // --- 1. PREPARACIÓN: OBTENER LOS IDs DE WORKSHIFT TYPE ---
            // Guardamos los IDs que necesitaremos para no buscarlos repetidamente.
            var dayId = workshiftTypes.FirstOrDefault(wst => wst.Code == "Day")?.Id ?? 0;
            var mixedDayNightId = workshiftTypes.FirstOrDefault(wst => wst.Code == "MixedDayNight")?.Id ?? 0;
            var mixedNightDayId = workshiftTypes.FirstOrDefault(wst => wst.Code == "MixedNightDay")?.Id ?? 0;
            var nightId = workshiftTypes.FirstOrDefault(wst => wst.Code == "Night")?.Id ?? 0;




            // --- 2. LÓGICA PARA HORARIO REGULAR (PayCategory == RegularShift) ---
            var regularSpans = subspans.Where(s => s.PayCategoryId == regularShiftId).ToList();
            if (regularSpans.Any())
            {
                decimal totalNightHoursRegular = regularSpans
                    .Where(s => s.SubTimeIn.Value.Hour >= 18 || s.SubTimeIn.Value.Hour < 6)
                    .Sum(s => s.ClockedHours);

                int regularWorkshiftTypeId;
                if (totalNightHoursRegular > 3)
                {
                    regularWorkshiftTypeId = nightId; // Más de 3h nocturnas -> Turno Nocturno
                }
                else if (totalNightHoursRegular > 0)
                {
                    // --- INICIO DE LA NUEVA LÓGICA PARA TURNOS MIXTOS ---

                    // 1. Encontramos el primer tramo para saber a qué hora empezó la jornada.
                    var firstRegularSpan = regularSpans.OrderBy(s => s.SubTimeIn).First();

                    // 2. Determinamos si ese primer tramo fue nocturno.
                    bool startedAtNight = firstRegularSpan.SubTimeIn.Value.Hour >= 18 || firstRegularSpan.SubTimeIn.Value.Hour < 6;

                    // 3. Asignamos el ID de mixto correspondiente.
                    if (startedAtNight)
                    {
                        regularWorkshiftTypeId = mixedNightDayId; // Empezó de noche y tuvo horas de día.
                    }
                    else
                    {
                        regularWorkshiftTypeId = mixedDayNightId; // Empezó de día y tuvo horas de noche.
                    }
                    // --- FIN DE LA NUEVA LÓGICA ---
                }
                else
                {
                    regularWorkshiftTypeId = dayId; // Cero horas nocturnas -> Turno Diurno
                }

                // Aplicamos el ID calculado a TODOS los spans de horario regular.
                foreach (var span in regularSpans)
                {
                    span.WorkshiftTypeId = regularWorkshiftTypeId;
                }
            }


            // --- 3. LÓGICA PARA SOBRETIEMPO (PayCategory = OvertimeShift) ---
            var overtimeSpans = subspans.Where(s => s.PayCategoryId == overtimeShiftId).OrderBy(s => s.SubTimeIn).ToList();
            if (overtimeSpans.Any())
            {
                // Usamos un 'enum' para manejar el estado de la clasificación de sobretiempo.
                OvertimeState overtimeState = OvertimeState.Initial;

                foreach (var span in overtimeSpans)
                {
                    // Determinamos si el span actual es diurno o nocturno.
                    bool isCurrentSpanNight = span.SubTimeIn.Value.Hour >= 18 || span.SubTimeIn.Value.Hour < 6;

                    switch (overtimeState)
                    {
                        case OvertimeState.Initial: // Para el primer span de sobretiempo
                            if (isCurrentSpanNight)
                            {
                                span.WorkshiftTypeId = nightId;
                                overtimeState = OvertimeState.Night;
                            }
                            else
                            {
                                span.WorkshiftTypeId = dayId;
                                overtimeState = OvertimeState.Day;
                            }
                            break;

                        case OvertimeState.Day: // Si el sobretiempo anterior fue diurno
                            if (isCurrentSpanNight) // Hubo un cambio de Diurno -> Nocturno
                            {
                                span.WorkshiftTypeId = mixedDayNightId;
                                overtimeState = OvertimeState.MixedDayNight; // El estado se "bloquea"
                            }
                            else
                            {
                                span.WorkshiftTypeId = dayId; // Sigue siendo diurno
                            }
                            break;

                        case OvertimeState.Night: // Si el sobretiempo anterior fue nocturno
                            if (!isCurrentSpanNight) // Hubo un cambio de Nocturno -> Diurno
                            {
                                span.WorkshiftTypeId = mixedNightDayId;
                                overtimeState = OvertimeState.MixedNightDay; // El estado se "bloquea"
                            }
                            else
                            {
                                span.WorkshiftTypeId = nightId; // Sigue siendo nocturno
                            }
                            break;

                        case OvertimeState.MixedDayNight: // Una vez que se vuelve mixto, se queda así
                            span.WorkshiftTypeId = mixedDayNightId;
                            break;

                        case OvertimeState.MixedNightDay: // Igual para el otro tipo de mixto
                            span.WorkshiftTypeId = mixedNightDayId;
                            break;
                    }
                }
            }

            return subspans;
        }






        public List<SubspanWithSpanDetailTypesPADto> DetermineOvertimeExcess(List<SubspanWithSpanDetailTypesPADto> subspans, decimal overtimeHours, List<PayCategoryDto> payCategories)
        {
            // --- Constantes para los límites ---
            const decimal DailyOvertimeLimit = 3.0m;
            const decimal WeeklyOvertimeLimit = 9.0m;

            // --- Inicialización ---
            var resultSpans = new List<SubspanWithSpanDetailTypesPADto>();
            var dailyOvertimeAccumulator = 0.0m;
            bool excessTriggered = false;

            var overtimeShiftId = payCategories.FirstOrDefault(pc => pc.Code == "OvertimeShift")?.Id ?? 0;
            if (overtimeShiftId == 0)
            {
                // Si no hay categoría de sobretiempo, no hay nada que procesar.
                return subspans;
            }

            // --- Procesamiento ---
            foreach (var subspan in subspans.OrderBy(s => s.SubTimeIn))
            {
                if (excessTriggered)
                {
                    if (subspan.PayCategoryId == overtimeShiftId)
                    {
                        subspan.OvertimeExcess = true;
                    }
                    resultSpans.Add(subspan);
                    continue;
                }

                if (subspan.PayCategoryId != overtimeShiftId)
                {
                    resultSpans.Add(subspan);
                    continue;
                }

                if (!subspan.SubTimeIn.HasValue || !subspan.SubTimeOut.HasValue)
                {
                    resultSpans.Add(subspan);
                    continue;
                }

                var currentDuration = (decimal)(subspan.SubTimeOut.Value - subspan.SubTimeIn.Value).TotalHours;
                var remainingDailyTime = DailyOvertimeLimit - dailyOvertimeAccumulator;
                var remainingWeeklyTime = WeeklyOvertimeLimit - overtimeHours;
                var allowableOvertimeDuration = Math.Min(remainingDailyTime, remainingWeeklyTime);
                allowableOvertimeDuration = Math.Max(0, allowableOvertimeDuration);

                if (currentDuration > allowableOvertimeDuration)
                {
                    // --- División del Subspan ---
                    excessTriggered = true;

                    if (allowableOvertimeDuration > 0)
                    {
                        var normalPart = CreateCopy(subspan);
                        var ticksToAdd = (long)(allowableOvertimeDuration * TimeSpan.TicksPerHour);
                        normalPart.SubTimeIn = subspan.SubTimeIn.Value; // Se mantiene el inicio original
                        normalPart.SubTimeOut = subspan.SubTimeIn.Value.AddTicks(ticksToAdd);
                        normalPart.OvertimeExcess = false;
                        // --- LÍNEA AÑADIDA ---
                        normalPart.ClockedHours = (decimal)(normalPart.SubTimeOut.Value - normalPart.SubTimeIn.Value).TotalHours;
                        resultSpans.Add(normalPart);
                    }

                    var excessPart = CreateCopy(subspan);
                    var ticksToAddForExcessStart = (long)(allowableOvertimeDuration * TimeSpan.TicksPerHour);
                    excessPart.SubTimeIn = subspan.SubTimeIn.Value.AddTicks(ticksToAddForExcessStart);
                    excessPart.SubTimeOut = subspan.SubTimeOut.Value; // Se mantiene el fin original
                    excessPart.OvertimeExcess = true;
                    // --- LÍNEA AÑADIDA ---
                    excessPart.ClockedHours = (decimal)(excessPart.SubTimeOut.Value - excessPart.SubTimeIn.Value).TotalHours;
                    resultSpans.Add(excessPart);
                }
                else
                {
                    subspan.OvertimeExcess = false;
                    resultSpans.Add(subspan);
                }

                dailyOvertimeAccumulator += currentDuration;
                overtimeHours += currentDuration;
            }

            return resultSpans;
        }









        public List<SubspanPADto> ExpandSpanDetailTypeSubspans(List<SubspanWithSpanDetailTypesPADto> processedSubspans, List<SpanDetailTypeDto> spanDetailTypes, string employeeTypeCode)
        {
            var finalExpandedList = new List<SubspanWithSpanDetailTypesPADto>();
            if (processedSubspans == null || !processedSubspans.Any())
            {
                return new List<SubspanPADto>();
            }

            var spanDetailTypeLookup = spanDetailTypes.ToDictionary(st => st.Code, st => st);
            var naSpanDetailTypeId = spanDetailTypeLookup.GetValueOrDefault("NA")?.Id ?? 1;

            // --- INICIO DE LA LÓGICA DINÁMICA ---

            // 1. (Optimización) Obtenemos las propiedades booleanas UNA SOLA VEZ, antes de entrar al bucle principal.
            //    Esto es mucho más eficiente que hacerlo por cada subspan.
            var premiumProperties = typeof(SubspanWithSpanDetailTypesPADto).GetProperties()
                .Where(prop =>
                    // Filtramos solo las que son de tipo 'bool'
                    prop.PropertyType == typeof(bool) &&
                    // Y cuyo nombre existe como un 'Code' en nuestros SpanDetailTypes.
                    // Esto evita que intentemos procesar otros booleanos como 'IsInSchedule'.
                    spanDetailTypeLookup.ContainsKey(prop.Name))
                .ToList();

            // --- FIN DE LA LÓGICA DINÁMICA ---

            foreach (var subspan in processedSubspans)
            {
                // 2. Añadir la versión "base" del tiempo trabajado (esto no cambia).
                subspan.SpanDetailTypeId = naSpanDetailTypeId;
                finalExpandedList.Add(subspan);

                if (employeeTypeCode == "Construction") 
                { 
                    // 3. Ahora, para cada subspan, iteramos sobre la lista de propiedades que encontramos dinámicamente.
                    foreach (var prop in premiumProperties)
                    {
                        // Obtenemos el valor booleano (true/false) de la propiedad para el 'subspan' actual.
                        bool hasPremium = (bool)prop.GetValue(subspan);

                        if (hasPremium)
                        {
                            // El nombre de la propiedad (ej. "Rain") es la clave.
                            var premiumSpanDetailType = spanDetailTypeLookup[prop.Name];

                            var premiumSubspan = CreateCopy(subspan);
                            premiumSubspan.DisplayText = premiumSpanDetailType.Name;
                            premiumSubspan.SpanDetailTypeId = premiumSpanDetailType.Id;
                            finalExpandedList.Add(premiumSubspan);
                        }
                    }
                }
            }

            return finalExpandedList.Select(subspan => new SubspanPADto
            {
                // Mapeamos todas las propiedades comunes desde SubspanDto
                SpanId = subspan.SpanId,
                DisplayText = subspan.DisplayText,
                SubTimeIn = subspan.SubTimeIn,
                SubTimeOut = subspan.SubTimeOut,
                PayCategoryId = subspan.PayCategoryId,
                WorkshiftTypeId = subspan.WorkshiftTypeId,
                SpanTypeId = subspan.SpanTypeId,
                SpanDetailTypeId = subspan.SpanDetailTypeId,
                ClockedHours = subspan.ClockedHours,
                CountryCode = subspan.CountryCode,
                // ... copia todas las demás propiedades base (Factor, Amount, etc.)

                // Mapeamos la propiedad específica del DTO final
                OvertimeExcess = subspan.OvertimeExcess

            }).ToList();
        }



        private SubspanWithSpanDetailTypesPADto CreateSubspanWithSpanDetailTypesFromSpan(SpanPADto originalSpan, DateTime startTime, DateTime endTime, int payCategoryId, int spanTypeId, string countryCode)
        {
            return new SubspanWithSpanDetailTypesPADto
            {
                SpanId = originalSpan.Id,
                SubTimeIn = startTime,
                SubTimeOut = endTime,
                PayCategoryId = payCategoryId,
                SpanTypeId = spanTypeId,
                CountryCode = countryCode,
                ClockedHours = (decimal)(endTime - startTime).TotalHours,
                Rain = originalSpan.Rain,
                RainStoppage = originalSpan.RainStoppage,
                Height125mOrUnder = originalSpan.Height125mOrUnder,
                HeightOver125m = originalSpan.HeightOver125m,
                Depth = originalSpan.Depth,
                Tunnel = originalSpan.Tunnel,
                Hammering = originalSpan.Hammering,
                Raking = originalSpan.Raking
                // Las demás propiedades (Factor, Amount, etc.) tomarán sus valores por defecto.
            };
        }

        private SubspanWithSpanDetailTypesPADto CreateCopy(SubspanWithSpanDetailTypesPADto original)
        {
            return new SubspanWithSpanDetailTypesPADto
            {
                SpanId = original.SpanId,
                DisplayText = original.DisplayText,
                SubTimeIn = original.SubTimeIn,
                SubTimeOut = original.SubTimeOut,
                PayCategoryId = original.PayCategoryId,
                WorkshiftTypeId = original.WorkshiftTypeId,
                SpanTypeId = original.SpanTypeId,
                SpanDetailTypeId = original.SpanDetailTypeId,
                Factor1 = original.Factor1,
                Factor2 = original.Factor2,
                Factor3 = original.Factor3,
                Factor4 = original.Factor4,
                Substract = original.Substract,
                Factor = original.Factor,
                Amount = original.Amount,
                ClockedHours = original.ClockedHours,
                MealHours = original.MealHours,
                CountryCode = original.CountryCode,
                Rain = original.Rain,
                RainStoppage = original.RainStoppage,
                Height125mOrUnder = original.Height125mOrUnder,
                HeightOver125m = original.HeightOver125m,
                Depth = original.Depth,
                Tunnel = original.Tunnel,
                Hammering = original.Hammering,
                Raking = original.Raking,
                OvertimeExcess = original.OvertimeExcess
            };
        }

    }
}
