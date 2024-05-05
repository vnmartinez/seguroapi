public interface ICarregaDados
{
  List<CarInsuranceModel> Search();
  List<T> Load<T>(string local);
}