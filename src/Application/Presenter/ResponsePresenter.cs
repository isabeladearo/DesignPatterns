using src.Infra.Presenter;

namespace src.Application.Presenter;

public class ResponsePresenter
{
    public static IPresenter Create(string responseType)
    {
        return responseType.Equals("csv_response") ? new CsvPresenter() : new JsonPresenter();
    }
}
