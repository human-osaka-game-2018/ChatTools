﻿using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using WPF_Core.Common;
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

        public ICommand KeyCheckCommand { get; set; }

        public TextAreaViewModel()
        {
            PostCommand = new DelegateCommand(_ => PostMessage());

            KeyCheckCommand = new DelegateCommand(_ => PostMessageOnCtrlEnter());
        }

        private void PostMessageOnCtrlEnter()
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Enter))
            {
                PostMessage();
            }
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
