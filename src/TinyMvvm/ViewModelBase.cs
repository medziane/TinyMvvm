﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm.IoC;
using TinyNavigationHelper;

namespace TinyMvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private INavigationHelper _navigation;
        
        public ViewModelBase()
        {

        }

        public ViewModelBase(INavigationHelper navigation)
        {
            _navigation = navigation;
        }

        public async virtual Task Initialize()
        {

        }

        public async virtual Task OnAppearing()
        {
         
        }

        public async virtual Task OnDisappearing()
        {

        }

        public object NavigationParameter { get; set; }

        public ICommand NavigateTo
        {
            get
            {
                return new TinyCommand<string>(async (key) =>
                {
                    await Navigation.NavigateToAsync(key);
                });
            }
        }

        public ICommand OpenModal
        {
            get
            {
                return new TinyCommand<string>(async (key) =>
                {
                    await Navigation.OpenModalAsync(key); 
                });
            }
        }

        public INavigationHelper Navigation
        {
            get
            {
                if (_navigation == null && Resolver.IsEnabled)
                {
                    return Resolver.Resolve<INavigationHelper>();
                }
                else if (_navigation != null)
                {
                    return _navigation;
                }

                throw new NullReferenceException("Please pass a INavigation implementation to the constructor");
            }
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotBusy");
            }
        }

        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }

      

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
