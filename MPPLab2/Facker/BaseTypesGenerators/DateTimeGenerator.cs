using System;
using System.Collections.Generic;
using System.Text;
using Facker;
namespace Facker.BaseTypesGenerators
{
    public class DateTimeGenerator : IGenerator
    {
        protected readonly Random random;
        // protected Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        public Type GeneratedType
        { get; protected set; }

        public object Create(Type type)
        {
            if (!IsTypeValid(type))
            {
                throw new Exception();
            }
            /* generated values are limited according to DateTime limitations */
            int year = random.Next(DateTime.MinValue.Year, DateTime.MaxValue.Year + 1);
            int month = random.Next(1, 13);
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
            int hour = random.Next(0, 24);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);
            int millisecond = random.Next(0, 1000);

            return new DateTime(year, month, day, hour, minute, second, millisecond);

        }

        public bool IsTypeValid(Type type)
        {
            return typeof(DateTime) == type;
        }
        public DateTimeGenerator()
        {
            GeneratedType = typeof(DateTime);
            random = new Random();
        }
        public Type GetGeneratedType()
        {
            return typeof(DateTime);
        }
    }
}
