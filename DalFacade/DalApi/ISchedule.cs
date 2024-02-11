namespace DalApi
{
    public interface ISchedule
    {
        public DateTime? SetStartProjectDate(DateTime date);
        public DateTime? getStartProjectDate();

        public DateTime? SetEndProjectDate(DateTime date);
        public DateTime? getEndProjectDate();

        public void resetTime();

       
    }
}