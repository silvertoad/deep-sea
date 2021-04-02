using System.Collections.Generic;

namespace Common.UI.DataGroup
{
    public interface IDataGroup<TDataType>
    {
        void SetData(IList<TDataType> itemData);
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType itemData);
    }
}