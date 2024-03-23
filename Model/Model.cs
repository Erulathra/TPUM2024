using Logic;

namespace Model
{
    public class Model
    {
        private LogicAbstractApi logicAbstractApi;

        public Model(LogicAbstractApi? logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi == null ? LogicAbstractApi.Create() : logicAbstractApi;
        }

    }
}
