﻿using System;
using System.Windows.Forms;

namespace Search3.Settings.UI
{
    public partial class KeywordsDialog : Form
    {
        #region Constructors

        private KeywordsDialog(KeywordsSettings settings)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.CuahsiIcon;
            Load += delegate { keywordsUserControl1.BindKeywordsSettings(settings); };
        }

        #endregion

        #region Public methods

        public static DialogResult ShowDialog(KeywordsSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            using(var form = new KeywordsDialog(settings.Copy()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    settings.SelectedKeywords = form.keywordsUserControl1.GetSelectedKeywords();
                }

                return form.DialogResult;
            }
        }

        #endregion
    }
}
