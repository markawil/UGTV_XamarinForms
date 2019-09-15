using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using UGTVForms.ViewModels;
using Xamarin.Forms;

namespace UGTVForms.Models
{
    public interface IDataStore<T>
    {
        bool AddItem(T item);
        bool DeleteItem(string id);
        T GetItem(string id);
        IEnumerable<T> All();
    }
}
