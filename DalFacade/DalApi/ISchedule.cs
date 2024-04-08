namespace DalApi
{
    public interface ISchedule
    {
        public DateTime? SetStartProjectDate(DateTime date);//Updates project start date
        public DateTime? getStartProjectDate();//Returns project start date

        public DateTime? SetEndProjectDate(DateTime date);//Updates project end date
        public DateTime? getEndProjectDate();//Returns project end date

        public void resetTime();//Resets dates
        public void resetRunNumber();//Resets run Number

    }
}