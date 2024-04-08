using BO;

namespace BlApi
{
    public interface ISchedule
    {
        public DateTime? SetStartProjectDate(DateTime date);
        public DateTime? getStartProjectDate();

        public DateTime? SetEndProjectDate(DateTime date);
        public DateTime? getEndProjectDate();

        public void resetTime();//Reset project dates
        public void resetRunNumber();//Reset running numbers
    }
}