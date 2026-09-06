using ClientManagement.Models.DataTransferObjects.Base;
using ClientManagement.Models.Interfaces.Base;
using Core.Presentation.ViewComponents.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ClientManagement.Models.Base
{
    public class GenericViewModel<TState> : IGenericViewModel<TState> where TState : BaseDto, new()
    {
        public event IGenericViewModel<TState>.ViewModelStateChangeDelegate? ViewModelStateChangeEvent;
        public GenericViewModel(TState viewModelState) { 
            this.ViewModelState = viewModelState;
        }

        public TState ViewModelState { get; set; }
        public dynamic? Get(string propName, Type propType, bool applyDisplayFormat = false)
        {
            var prop = this.ViewModelState.GetType()?.GetProperty(propName);
            var display =  prop?.GetCustomAttribute<DisplayFormatAttribute>();
            var propVal = prop?.GetValue(this.ViewModelState);
            if (display is { DataFormatString : string } && applyDisplayFormat && propType == typeof(string))
            {
                return string.Format(display.DataFormatString, propVal);
            }
            else
            {
                return this.ConvertSource(propType, propVal);
            }
          
          
        }

        public void Set<TSource>(string propName, TSource propValue)
        {
            var prop = this.ViewModelState?.GetType().GetProperty(propName);
           
            if (prop is PropertyInfo propInfo and { CanWrite: true, SetMethod.IsPublic: true }
            && (this.ViewModelState is not null) && propValue is object)
            {
                propInfo.SetValue(this.ViewModelState, this.ConvertSource(propInfo.PropertyType, propValue));
                if (this.ViewModelStateChangeEvent != null)
                {
                    this.ViewModelStateChangeEvent(this.ViewModelState);
                }
            }
        }

        public dynamic? ConvertSource<TTarget>(TTarget targetType, object? source) where TTarget : Type 
        {
           var stringType = typeof(string).FullName;
           var intType = typeof(int).FullName;  
           var boolType = typeof(bool).FullName;
           var doubletype = typeof(double).FullName;
           var guidType = typeof(Guid).FullName;
           var dateTimeType = typeof(DateTime).FullName;
           var nullableDateTime = typeof(DateTime?).FullName;
           var strCollectionType = typeof(IEnumerable<string>).FullName;
           var targetTypeName = targetType.FullName;
           if(targetTypeName == stringType && source is object validSource)
            {
               // var isGuid = Guid.TryParse(validSource.ToString(), out var guidResult);
                return Convert.ToString(validSource);
            }
            else if (targetTypeName == boolType && source is object validBoolSource)
            {
                _ = bool.TryParse(validBoolSource.ToString(), out var boolResult);
                return boolResult;
            }
            else if (targetTypeName == intType && source is object validIntSource)
            {
                _ = int.TryParse(validIntSource.ToString(), out var intResult);
                return intResult;
            }
           else if(targetTypeName == doubletype && source is object validDoubleSource)
            {
                _ = double.TryParse(validDoubleSource.ToString(), out var doubleResult);
                return doubleResult;
            }
           else if(targetTypeName == strCollectionType && source is IEnumerable<string> validStrCollectionSource)
            {
                return validStrCollectionSource;
            }
           else if(targetTypeName == guidType && source is object validSourceObject)
           {
                _ = Guid.TryParse(validSourceObject.ToString(), out var guidResult);
                return guidResult;
           }
            else if ((targetTypeName == dateTimeType || targetTypeName == nullableDateTime) && source is object validDateSource)
            {
                var match = Regex.Match(validDateSource.ToString(), @"(\d{2})/(\d{2})/(\d{4})");
                if (match.Success)
                {
                    return new DateTime(
                        Convert.ToInt32(match.Groups[3].Value),
                        Convert.ToInt32(match.Groups[2].Value),
                        Convert.ToInt32(match.Groups[1].Value));
                }
                else
                {
                    return targetTypeName == nullableDateTime ? null : DateTime.MinValue;
                }
            }
            else
            {
                return null;
            }
        }

        public void AddStateChangeListener(IGenericViewModel<TState>.ViewModelStateChangeDelegate stateChangeListener)
        {
            this.ViewModelStateChangeEvent += stateChangeListener;
        }

        public void ClearStateChangeListeners()
        {
            this.ViewModelStateChangeEvent = null;
        }

        public void Set(string key, object? val, IBindToSetterDelegateBase<string, object?, TState>.BindToSetterDelegate setter)
        {
            setter(key, val, this.ViewModelState);
        }

        public Action<TKey> Set<TKey, TValue>(TValue value) 
        {
            return (TKey key) =>
            {
                this.Set(key as string, value);
            };
        }


    }
}
