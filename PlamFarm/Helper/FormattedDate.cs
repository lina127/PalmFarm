namespace PlamFarm.Helper
{
    public static class FormattedDate
    {
        public static string ToFormattedDateTime(DateTime date)
        {
            //return date.Year + "년 " + date.Month + "월 " + date.Day + "일";
            return date.Year + "/ " + date.Month + "/ " + date.Day + "";
        }
    }
}
