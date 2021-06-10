namespace Avelraangame.Services.Base
{
    public class ItemBase
    {
        protected DataService DataService { get; set; }

        protected ItemBase()
        {
            DataService = new DataService();
        }


    }
}
