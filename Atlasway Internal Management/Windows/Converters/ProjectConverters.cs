using Atlasway_Internal_Management.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Atlasway_Internal_Management.Windows.Converters;


public class ProjectClientNoToNameConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
        {
            throw new ArgumentException(
                message: $"The values array passed to {GetType().Name} must contain two items.",
                paramName: nameof(values));
        }

        object value0 = values[0];
        object value1 = values[1];

        if (value0 is null
            || value0 == BindingOperations.DisconnectedSource
            || value0 == DependencyProperty.UnsetValue
            || value1 is null
            || value1 == BindingOperations.DisconnectedSource
            || value1 == DependencyProperty.UnsetValue)
        {
            return DependencyProperty.UnsetValue;
        }
        else if (value0 is not Project project)
        {
            throw new ArgumentException($"The first value passed to {GetType().Name} must be of type {typeof(Project).Name}.", nameof(values));
        }
        else if (value1 is not List<Client> clients)
        {
            throw new ArgumentException($"The second value passed to {GetType().Name} must be of type {typeof(List<Client>).Name}.", nameof(values));
        }
        else
        {
            try
            {
                Client targetClient = clients.Where(client => client.ClientNo == project.ClientNo).FirstOrDefault();

                return $"{targetClient.ClientName}";
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException(
                    message: 
                        $"The items in the first value passed to {GetType().Name} must be of type {typeof(Project).Name}.\n" +
                        $"The items in the second value passed to {GetType().Name} must be of type {typeof(List<Client>).Name}.\n",
                    paramName: nameof(values),
                    innerException: ex);
            }
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ProjectStatusNoToNameConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
        {
            throw new ArgumentException(
                message: $"The values array passed to {GetType().Name} must contain two items.",
                paramName: nameof(values));
        }

        object value0 = values[0];
        object value1 = values[1];

        if (value0 is null
            || value0 == BindingOperations.DisconnectedSource
            || value0 == DependencyProperty.UnsetValue
            || value1 is null
            || value1 == BindingOperations.DisconnectedSource
            || value1 == DependencyProperty.UnsetValue)
        {
            return DependencyProperty.UnsetValue;
        }
        else if (value0 is not Project project)
        {
            throw new ArgumentException($"The first value passed to {GetType().Name} must be of type {typeof(Project).Name}.", nameof(values));
        }
        else if (value1 is not List<ProjectStatusType> projectStatusTypes)
        {
            throw new ArgumentException($"The second value passed to {GetType().Name} must be of type {typeof(List<ProjectStatusType>).Name}.", nameof(values));
        }
        else
        {
            try
            {
                ProjectStatusType status = projectStatusTypes.Where(projectStatus => projectStatus.TypeNo == project.StatusNo).FirstOrDefault();

                return $"{status.TypeName}";
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException(
                    message:
                        $"The items in the first value passed to {GetType().Name} must be of type {typeof(Project).Name}.\n" +
                        $"The items in the second value passed to {GetType().Name} must be of type {typeof(List<ProjectStatusType>).Name}.\n",
                    paramName: nameof(values),
                    innerException: ex);
            }
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
