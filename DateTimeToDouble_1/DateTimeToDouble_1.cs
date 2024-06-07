using System;
using Skyline.DataMiner.Analytics.GenericInterface;

[GQIMetaData(Name = "DateTimeToDouble")]
public class DateTimeToDouble : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
{
    private readonly GQIColumnDropdownArgument _dateTimeColumnArg = new GQIColumnDropdownArgument("Date Time Column") { IsRequired = true, Types = new GQIColumnType[] { GQIColumnType.DateTime } };
    private GQIColumn<double> _doubleColumn;
    private GQIColumn<DateTime> _dateTimeColumn;

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _dateTimeColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _dateTimeColumn = args.GetArgumentValue(_dateTimeColumnArg) as GQIColumn<DateTime>;
        if (_dateTimeColumn != null)
            _doubleColumn = new GQIDoubleColumn($"{_dateTimeColumn.Name} (as double)");

        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        if (_doubleColumn != null)
            header.AddColumns(_doubleColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        if (_dateTimeColumn == null || _doubleColumn == null)
            return;

        DateTime date = row.GetValue<DateTime>(_dateTimeColumn);
        row.SetValue(_doubleColumn, date.ToOADate());
    }
}