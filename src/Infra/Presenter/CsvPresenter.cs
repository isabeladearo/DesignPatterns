using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using src.Application.Presenter;
using src.Domain;

namespace src.Infra.Presenter;

public class CsvPresenter : IPresenter
{
    public object Present(List<Invoice> output)
    {
        var cvsContent = new StringBuilder();

        for (int i = 0; i < output.Count; i++)
        {
            var x = output[i];
            cvsContent.Append($"{x.Date};{x.Amount}");

            if (i != output.Count - 1)
                cvsContent.Append(Environment.NewLine);
        }

        return cvsContent.ToString();
    }
}
