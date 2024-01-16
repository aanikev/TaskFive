class Program
{
    static void Main(string[] args)
    {
        var VacationDictionary = new Dictionary<string, List<DateTime>>()
        {
            ["Иванов Иван Иванович"] = new List<DateTime>(),
            ["Петров Петр Петрович"] = new List<DateTime>(),
            ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
            ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
            ["Павлов Павел Павлович"] = new List<DateTime>(),
            ["Георгиев Георг Георгиевич"] = new List<DateTime>()
        };
        var workingDaysWithoutWeekends = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        List<DateTime> SetDateList = new List<DateTime>();

        //полученик дат отпусков сотрудников
        GetDateVacantion(VacationDictionary, workingDaysWithoutWeekends);

        //вывод в консоль
        foreach (var VacationList in VacationDictionary)
        {
            SetDateList = VacationList.Value;
            Console.WriteLine("\nДни отпуска " + VacationList.Key + " : \n");
            for (int i = 0; i < SetDateList.Count; i++) { Console.WriteLine(SetDateList[i]); }
        }
        Console.ReadKey();
    }
    public static void GetDateVacantion(Dictionary<string, List<DateTime>> VacationDictionary, List<string> workingDays)
    {
        int AllVacationCount = 0;
        List<DateTime> Vacations = new List<DateTime>();

        //инициализация дат + дней для отпусков
        DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime endOfYear = new DateTime(DateTime.Today.Year, 12, 31);
        Random gen = new Random();
        int range = (endOfYear - startOfYear).Days;

        //перебор сотрудников для генерации дат отпусков
        foreach (var emploeeVacationList in VacationDictionary)
        {
            //обозначаем что у каждого сотрудника отпуск по 28 дней, соответственно пока они не будут исчерпаны не выходим из цикла
            int vacationCount = 28;
            while (vacationCount > 0)
            {
                //генерация рандомной даты в году
                var startDate = startOfYear.AddDays(gen.Next(range));

                //проверяем что не выходные
                if (workingDays.Contains(startDate.DayOfWeek.ToString()))
                {
                    int[] vacationSteps = { 7, 14 };

                    int vacIndex = gen.Next(vacationSteps.Length);

                    var endDate = new DateTime();
                    int difference = 0;

                    if (vacationCount <= 7)
                    {
                        endDate = startDate.AddDays(7);
                        difference = 7;
                    }
                    else
                    {
                        switch (vacationSteps[vacIndex])
                        {

                            case (7):
                                endDate = startDate.AddDays(7);
                                difference = 7;
                                break;
                            case (14):
                                endDate = startDate.AddDays(14);
                                difference = 14;
                                break;
                            default:
                                break;
                        }
                    }

                    // Проверка условий по отпуску
                    bool CanCreateVacation = false;
                    bool existStart = false;
                    bool existEnd = false;
                    if (!Vacations.Any(element => element >= startDate && element <= endDate))
                    {
                        if (!Vacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate))
                        {
                            existStart = emploeeVacationList.Value.Any(element => element.AddMonths(1) >= startDate && element.AddMonths(1) >= endDate);
                            existEnd = emploeeVacationList.Value.Any(element => element.AddMonths(-1) <= startDate && element.AddMonths(-1) <= endDate);
                            if (!existStart || !existEnd)
                                CanCreateVacation = true;
                        }
                    }

                    if (CanCreateVacation)
                    {
                        for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                        {
                            Vacations.Add(dt);
                            emploeeVacationList.Value.Add(dt);
                        }
                        AllVacationCount++;
                        vacationCount -= difference;
                    }
                }
            }
        }
    }
}