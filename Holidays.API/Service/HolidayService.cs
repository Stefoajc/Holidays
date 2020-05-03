using Holidays.API.Common;
using Holidays.API.Models;
using Holidays.API.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holidays.API.Service
{
    public class HolidayService
    {
        #region Public Methods

        public Result<HolidayOutputModel?> GetHoliday(int dayOfMonth, int month, int? year = null)
        {
            var searchedYear = year == null || year == 0 ? DateTime.Now.Year : (int)year;

            if (IsMonthInRange(month))
                return new Result<HolidayOutputModel?>(new[] { "Месеците са извън границите 1-12" });

            if (IsDayInRange(dayOfMonth, month, searchedYear))
                return new Result<HolidayOutputModel?>(new[] { "Денят е извън диапазнона за избраният месец" });

            HolidayOutputModel? holidayOutputModel = GetAll(searchedYear)
                .FirstOrDefault(h => h.Date.Day == dayOfMonth && h.Date.Month == month);

            return new Result<HolidayOutputModel?>(holidayOutputModel);
        }

        public Result<bool> IsHoliday(int dayOfMonth, int month, int? year = null)
        {
            var searchedYear = year ?? DateTime.Now.Year;

            var result = GetHoliday(dayOfMonth, month, year);

            return result.Select(r => r != null);
        }

        public HolidayOutputModel? GetNextHoliday(DateTime? date = null)
        {
            var dateToGetHolidayAfter = date ?? DateTime.Now;

            HolidayOutputModel? constantHoliday = GetConstantHolidays
                .Select(ch => new HolidayOutputModel(ch, dateToGetHolidayAfter.Year))
                .FirstOrDefault(h => h.Date.Day > dateToGetHolidayAfter.Day && h.Date.Month >= dateToGetHolidayAfter.Month);

            HolidayOutputModel? floatingHoliday = GetFloatingHolidays(dateToGetHolidayAfter.Year)
                .Select(fh => new HolidayOutputModel(fh))
                .FirstOrDefault(h => h.Date.Day > dateToGetHolidayAfter.Day && h.Date.Month >= dateToGetHolidayAfter.Month);

            return (constantHoliday, floatingHoliday) switch
            {
                var tuple when tuple.constantHoliday != null && tuple.floatingHoliday != null
                    => tuple.floatingHoliday.Date < tuple.constantHoliday.Date
                        ? tuple.floatingHoliday
                        : tuple.constantHoliday,
                var tuple when tuple.constantHoliday != null && tuple.floatingHoliday == null
                    => tuple.constantHoliday,
                var tuple when tuple.constantHoliday == null && tuple.floatingHoliday != null
                    => tuple.floatingHoliday,
                _ => null,
            };
        }

        internal object GetAll(int? year) => throw new NotImplementedException();

        public IEnumerable<HolidayOutputModel> GetAll(int year, int? month = null)
        {
            var holidays = GetConstantHolidays
                .Select(ch => new HolidayOutputModel(ch, year))
                .Union(GetFloatingHolidays(year)
                    .Select(fh => new HolidayOutputModel(fh)))
                .OrderBy(h => h.Date);

            return month == null
                ? holidays
                : holidays
                    .Where(h => h.Date.Month == month);
        }

        #endregion

        #region Private Methods        

        private readonly List<ConstantHoliday> GetConstantHolidays = new List<ConstantHoliday>
                {
                    new ConstantHoliday
                    {
                       Month = 1,
                       DayOfMonth = 1,
                       Name = "Имен ден - Васил"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 2,
                        Name = "Имен ден - Гopан, Гopица"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 6,
                        Name = "Имен ден - Йордан"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 7,
                        Name = "Имен ден - Иван"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 12,
                        Name = "Имен ден - Таня, Татяна"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 17,
                        Name = "Имен ден - Антон"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 18,
                        Name = "Имен ден - Атанас"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 20,
                        Name = "Имен ден - Евтим"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 21,
                        Name = "Имен ден - Агнеса, Максим"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 22,
                        Name = "Имен ден - Тимотей, Тимофей"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 24,
                        Name = "Имен ден - Аксения, Ксения"
                    },
                    new ConstantHoliday
                    {
                        Month = 1,
                        DayOfMonth = 25,
                        Name = "Имен ден - Григор, Григории"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 1,
                        Name = "Имен ден - Трифон"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 3,
                        Name = "Имен ден - Симеон"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 5,
                        Name = "Имен ден - Агата, Агатия"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 6,
                        Name = "Имен ден - Пламена, Огнян"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 10,
                        Name = "Имен ден - Харалампи, Ламби, Валентин"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 13,
                        Name = "Имен ден - Евлоги, Евлогия"
                    },
                    new ConstantHoliday
                    {
                        Month = 2,
                        DayOfMonth = 21,
                        Name = "Имен ден - Евстати, Евстатия"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 4,
                        Name = "Имен ден - Герасим, Герасимка"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 9,
                        Name = "Имен ден - Младен, Младена"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 10,
                        Name = "Имен ден - Галина, Галя"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 16,
                        Name = "Имен ден - Тодор"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 19,
                        Name = "Имен ден - Дарина, Дария"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 21,
                        Name = "Имен ден - Яков"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 23,
                        Name = "Имен ден - Лидия"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 25,
                        Name = "Имен ден (Благовещение) - Благой, Благо"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 26,
                        Name = "Имен ден - Гавраил, Гаврил"
                    },
                    new ConstantHoliday
                    {
                        Month = 3,
                        DayOfMonth = 28,
                        Name = "Имен ден - Боян, Бояна, Бойко, Албена"
                    },
                    new ConstantHoliday
                    {
                        Month = 4,
                        DayOfMonth = 4,
                        Name = "Имен ден - Аврам"
                    },
                    new ConstantHoliday
                    {
                        Month = 4,
                        DayOfMonth = 14,
                        Name = "Имен ден - Мартина, Мартина"
                    },
                    new ConstantHoliday
                    {
                        Month = 4,
                        DayOfMonth = 20,
                        Name = "Имен ден - Лазар"
                    },
                    new ConstantHoliday
                    {
                        Month = 4,
                        DayOfMonth = 25,
                        Name = "Имен ден - Марко"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 1,
                        Name = "Имен ден - Йеремия"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 2,
                        Name = "Имен ден - Борис, Борислава"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 3,
                        Name = "Имен ден (Светли петък, Живоприемни източник) - Живко, Живка"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 5,
                        Name = "Имен ден - Ирена, Ирина"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 6,
                        Name = "Имен ден - Георги, Гергана"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 11,
                        Name = "Имен ден - Томина, Томислав"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 12,
                        Name = "Имен ден - Герман"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 21,
                        Name = "Имен ден - Константин, Елена"
                    },
                    new ConstantHoliday
                    {
                        Month = 5,
                        DayOfMonth = 30,
                        Name = "Имен ден - Емил, Емилия, Емилиян"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 6,
                        Name = "Имен ден - Спас"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 7,
                        Name = "Имен ден - Валери, Валерия"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 17,
                        Name = "Имен ден - Мануил, Емануил"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 20,
                        Name = "Имен ден - Бисер, Бисера"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 23,
                        Name = "Имен ден - Асен, Ася, Аспарух, Десислава, Панайот, Крум, Румен, Чавдар"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 24,
                        Name = "Имен ден - Еньо"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 29,
                        Name = "Имен ден - Петър, Петра"
                    },
                    new ConstantHoliday
                    {
                        Month = 6,
                        DayOfMonth = 30,
                        Name = "Имен ден - Апостол"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 1,
                        Name = "Имен ден - Кузман, Дамян, Красимир, Красимира"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 5,
                        Name = "Имен ден - Атанас, Атанаска, Наско"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 7,
                        Name = "Имен ден - Недялко, Недялка"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 15,
                        Name = "Имен ден - Владимир, Владимира, Влади"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 16,
                        Name = "Имен ден - Олга, Оля, Юлия, Юлиан"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 17,
                        Name = "Имен ден - Марин, Марина"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 20,
                        Name = "Имен ден (Илинден) - Илия, Илиян, Илияна, Илко, Илка"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 22,
                        Name = "Имен ден - Магда, Магдалена"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 25,
                        Name = "Имен ден - Ана, Анна"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 26,
                        Name = "Имен ден - Параскев, Параскева"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 27,
                        Name = "Имен ден (Пантелей пътник) - Пантелей"
                    },
                    new ConstantHoliday
                    {
                        Month = 7,
                        DayOfMonth = 31,
                        Name = "Имен ден - Евдоким, Евдокия"
                    },
                    new ConstantHoliday
                    {
                        Month = 8,
                        DayOfMonth = 8,
                        Name = "Имен ден - Емилия, Емилиян"
                    },
                    new ConstantHoliday
                    {
                        Month = 8,
                        DayOfMonth = 15,
                        Name = "Имен ден (Голяма Богородица) - Мария, Мариана, Марияна, Марко, Деспина"
                    },
                    new ConstantHoliday
                    {
                        Month = 8,
                        DayOfMonth = 20,
                        Name = "Имен ден - Самуил"
                    },
                    new ConstantHoliday
                    {
                        Month = 8,
                        DayOfMonth = 26,
                        Name = "Имен ден - Адриан, Адриана"
                    },
                    new ConstantHoliday
                    {
                        Month = 8,
                        DayOfMonth = 30,
                        Name = "Имен ден - Александър, Алекс"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 1,
                        Name = "Имен ден - Симеон"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 3,
                        Name = "Имен ден - Антим"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 5,
                        Name = "Имен ден - Захари, Захария, Зара, Елисавета"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 14,
                        Name = "Имен ден (Кръстовден) - Кръстьо"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 16,
                        Name = "Имен ден - Людмил, Людмила"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 17,
                        Name = "Имен ден - Вяра, Надежда, Надя, Любов, Люба, Любен, Любомир, София, Софка"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 22,
                        Name = "Имен ден - Гълъбин, Гълъбина"
                    },
                    new ConstantHoliday
                    {
                        Month = 9,
                        DayOfMonth = 25,
                        Name = "Имен ден - Сергей"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 1,
                        Name = "Имен ден - Анани, Анания"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 6,
                        Name = "Имен ден - Тома, Томина"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 7,
                        Name = "Имен ден - Сергей 2"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 9,
                        Name = "Имен ден - Аврам"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 14,
                        Name = "Имен ден - Петко, Петка"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 18,
                        Name = "Имен ден - Златка, Златко, Златина"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 26,
                        Name = "Имен ден - Димитър"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 27,
                        Name = "Имен ден - Нестор"
                    },
                    new ConstantHoliday
                    {
                        Month = 10,
                        DayOfMonth = 28,
                        Name = "Имен ден - Лъчезар, Лъчезара"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 1,
                        Name = "Имен ден - Аргир"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 8,
                        Name = "Имен ден (Архангеловден) - Ангел, Архангел"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 11,
                        Name = "Имен ден - Мина, Минка"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 13,
                        Name = "Имен ден - Евлоги, Евлогия"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 14,
                        Name = "Имен ден - Филип"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 16,
                        Name = "Имен ден - Матей"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 24,
                        Name = "Имен ден - Екатерина, Катя"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 25,
                        Name = "Имен ден - Климент, Климентина"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 26,
                        Name = "Имен ден - Стилиан, Стилян, Стиляна"
                    },
                    new ConstantHoliday
                    {
                        Month = 11,
                        DayOfMonth = 30,
                        Name = "Имен ден - Андрей, Андреа, Андриан, Първан"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 4,
                        Name = "Имен ден - Варвара, Варадин"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 5,
                        Name = "Имен ден - Сава, Сафка, Елисавета, Славчо"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 6,
                        Name = "Имен ден (Никулден) - Николай, Николета"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 8,
                        Name = "Имен ден - Стамат"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 8,
                        Name = "Имен ден - Ана"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 12,
                        Name = "Имен ден - Спиридон"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 14,
                        Name = "Имен ден - Снежа, Снежана"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 15,
                        Name = "Имен ден - Свобода, Елевтер"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 17,
                        Name = "Имен ден - Данаил, Данаила"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 18,
                        Name = "Имен ден - Модест"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 20,
                        Name = "Имен ден - Пламен, Огнян, Игнат, Иго"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 22,
                        Name = "Имен ден - Анастасия"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 26,
                        Name = "Имен ден - Давид, Йосиф"
                    },
                    new ConstantHoliday
                    {
                        Month = 12,
                        DayOfMonth = 27,
                        Name = "Имен ден - Стефан, Стоян, Станчо, Венцислав, Запрян, Стамен"
                    }
            };

        private List<FloatingHoliday> GetFloatingHolidays(int year)
        {
            DateTime GetEasterSunday(int year)
            {
                int month = 0;
                int day = 0;
                int g = year % 19;
                int c = year / 100;
                int h = h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
                int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

                day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
                month = 3;

                if (day > 31)
                {
                    month++;
                    day -= 31;
                }

                return new DateTime(year, month, day);
            }

            DateTime GetGoodFriday(int year) => GetEasterSunday(year).AddDays(-2);

            DateTime GetAscensionDay(int year) => GetEasterSunday(year).AddDays(39);

            DateTime GetWhitSunday(int year) => GetEasterSunday(year).AddDays(49);

            DateTime GetPalmDate(int year) => GetEasterSunday(year).AddDays(-7);

            return new List<FloatingHoliday>
            {
                new FloatingHoliday
                {
                    Date = GetEasterSunday(year),
                    Name = "Великден"
                },
                new FloatingHoliday
                {
                    Date = GetGoodFriday(year),
                    Name = "Разпети петък"
                },
                new FloatingHoliday
                {
                    Date = GetAscensionDay(year),
                    Name = "Възнесение господне"
                },
                new FloatingHoliday
                {
                    Date = GetWhitSunday(year),
                    Name = "Петдесетница"
                },
                new FloatingHoliday
                {
                    Date = GetPalmDate(year),
                    Name = "Цветница"
                }
            };
        }

        private static bool IsDayInRange(int dayOfMonth, int month, int year) => DateTime.DaysInMonth(year, month) < dayOfMonth || dayOfMonth < 1;
        private static bool IsMonthInRange(int month) => month < 1 && month > 12;

        #endregion
    }
}
