﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Core.Common;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class TextAreaViewModel : BindableBase
    {
        public string Text 
        {
            get => text;
            set => SetValue(ref text, value);
        }

        public ICommand PostCommand { get; set; }

        public TextAreaViewModel()
        {
            PostCommand = new DelegateCommand(_ => PostMessage());
        }

        private void PostMessage()
        {
            if (ChannelService.SelectingChannel is null) return;

            if (String.IsNullOrWhiteSpace(Text)) return;

            MessageService.Post(Text, LogInService.LogInUser!, ChannelService.SelectingChannel);

            Text = "";
        }

        private string text = "";
    }
}