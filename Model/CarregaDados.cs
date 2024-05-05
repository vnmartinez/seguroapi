using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class CarregaDadosCSV : ICarregaDados
{
    public List<CarInsuranceModel> Search()
    {
        return Load<CarInsuranceModel>(@Constants.CAMINHO_ARQUIVO);
    }

    public List<T> Load<T>(string local)
{
    if (!File.Exists(local))
        throw new ArgumentException(local);

    using (var reader = new StreamReader(local))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        csv.Context.RegisterClassMap<CarInsuranceModelMap>();
        return csv.GetRecords<T>().ToList();
    }
}
}

public class CarInsuranceModelMap : ClassMap<CarInsuranceModel>
{
    public CarInsuranceModelMap()
    {
        Map(m => m.ID).Name("ID");
        Map(m => m.Age).Name("AGE");
        Map(m => m.Gender).Name("GENDER");
        Map(m => m.Race).Name("RACE");
        Map(m => m.DrivingExperience).Name("DRIVING_EXPERIENCE");
        Map(m => m.Education).Name("EDUCATION");
        Map(m => m.Income).Name("INCOME");
        Map(m => m.CreditScore).Name("CREDIT_SCORE");
        Map(m => m.VehicleOwnership).Name("VEHICLE_OWNERSHIP");
        Map(m => m.VehicleYear).Name("VEHICLE_YEAR");
        Map(m => m.Married).Name("MARRIED");
        Map(m => m.Children).Name("CHILDREN");
        Map(m => m.PostalCode).Name("POSTAL_CODE");
        Map(m => m.AnnualMileage).Name("ANNUAL_MILEAGE");
        Map(m => m.VehicleType).Name("VEHICLE_TYPE");
        Map(m => m.SpeedingViolations).Name("SPEEDING_VIOLATIONS");
        Map(m => m.DUIs).Name("DUIS");
        Map(m => m.PastAccidents).Name("PAST_ACCIDENTS");
        Map(m => m.Outcome).Name("OUTCOME");
    }
}