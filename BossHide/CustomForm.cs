﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PrivacyHide
{
    public class CustomForm : System.Windows.Forms.Form
    {
        public CustomForm()
        {
            InitializeComponent();
        }

        /*
        protected override void OnPaintBackground(PaintEventArgs e)
        {

            try
            {
                System.Drawing.Graphics g = e.Graphics;
                int x = this.Width;
                int y = this.Height;

                System.Drawing.Drawing2D.LinearGradientBrush
                    lgBrush = new System.Drawing.Drawing2D.LinearGradientBrush
                    (new System.Drawing.Point(0, 0), new System.Drawing.Point(x, y),
                    Color.White, Color.FromArgb(190, 190, 190));
                lgBrush.GammaCorrection = true;
                g.FillRectangle(lgBrush, 0, 0, x, y);

            }
            catch
            {
                base.OnPaintBackground(e);
            }
        }
        */

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomForm));
            this.SuspendLayout();
            // 
            // CustomForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.CustomForm_Load);
            this.ResumeLayout(false);

        }

        private void CustomForm_Load(object sender, EventArgs e)
        {
            //if (!(this is frmMain))
            //{
                //this.ShowInTaskbar = false;
            //}
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.ResizeControls();
        }

        private System.Windows.Forms.ToolTip tooltip = null;

        public void ResizeControls()
        {
            tooltip = new ToolTip();

            if (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() != "")
            {
                for (int k = 0; k < this.Controls.Count; k++)
                {
                    this.Controls[k].AccessibleName = this.Controls[k].Name;
                    this.Controls[k].AccessibleDescription = this.Controls[k].Text;
                }

                for (int k = 0; k < this.Controls.Count; k++)
                {
                    ResizeChildControls(this.Controls[k]);

                    if (!(this.Controls[k] is CheckBox
                        || this.Controls[k] is Label
                        || this.Controls[k] is RadioButton
                        ))
                        continue;

                    int left = this.Controls[k].Left;
                    int right = this.Controls[k].Right;
                    int top = this.Controls[k].Top;
                    int bottom = this.Controls[k].Bottom;
                    int height = this.Controls[k].Height;

                    for (int j = 0; j < this.Controls.Count; j++)
                    {
                        if (k == j) continue;

                        if (this.Controls[j].Left > this.Controls[k].Left
                            && this.Controls[k].Right >= this.Controls[j].Left
                            && ((this.Controls[j].Top >= this.Controls[k].Top
                            && this.Controls[j].Bottom >= this.Controls[k].Bottom
                            && this.Controls[j].Top <= this.Controls[k].Bottom
                            )
                            || (this.Controls[j].Top <= this.Controls[k].Top
                            && this.Controls[j].Bottom >= this.Controls[k].Bottom

                            )
                            ))
                        {
                            //this.Controls[k].Width = this.Controls[j].Left - this.Controls[k].Left - 5;

                            if (this.Controls[k] is CheckBox)
                            {
                                CheckBox chk = this.Controls[k] as CheckBox;

                                chk.AutoSize = false;
                                chk.Left = left;
                                chk.Top = top;
                                chk.Width = this.Controls[j].Left - this.Controls[k].Left - 5;
                                chk.Height = height;
                                chk.AutoEllipsis = true;
                            }
                            else if (this.Controls[k] is Label)
                            {
                                Label chk = this.Controls[k] as Label;

                                chk.AutoSize = false;
                                chk.Left = left;
                                chk.Top = top;
                                chk.Width = this.Controls[j].Left - this.Controls[k].Left - 5;
                                chk.Height = height;
                                chk.AutoEllipsis = true;
                            }
                            else if (this.Controls[k] is RadioButton)
                            {
                                RadioButton chk = this.Controls[k] as RadioButton;

                                chk.AutoSize = false;
                                chk.Left = left;
                                chk.Top = top;
                                chk.Width = this.Controls[j].Left - this.Controls[k].Left - 5;
                                chk.Height = height;
                                chk.AutoEllipsis = true;
                            }

                            tooltip.SetToolTip(this.Controls[k], this.Controls[k].Text);
                        }
                    }
                }
            }
        }

        private void ResizeChildControls(Control co)
        {
            for (int k = 0; k < co.Controls.Count; k++)
            {
                co.Controls[k].AccessibleName = co.Controls[k].Name;
                co.Controls[k].AccessibleDescription = co.Controls[k].Text;
            }

            for (int k = 0; k < co.Controls.Count; k++)
            {
                ResizeChildControls(co.Controls[k]);

                if (!(co.Controls[k] is CheckBox
                        || co.Controls[k] is Label
                        || co.Controls[k] is RadioButton
                        ))
                    continue;

                int left = co.Controls[k].Left;
                int right = co.Controls[k].Right;
                int top = co.Controls[k].Top;
                int bottom = co.Controls[k].Bottom;
                int height = co.Controls[k].Height;

                for (int j = 0; j < co.Controls.Count; j++)
                {
                    if (k == j) continue;

                    if (co.Controls[j].Left > co.Controls[k].Left
                            && co.Controls[k].Right >= co.Controls[j].Left
                            && ((co.Controls[j].Top >= co.Controls[k].Top
                            && co.Controls[j].Bottom >= co.Controls[k].Bottom
                            && co.Controls[j].Top <= co.Controls[k].Bottom
                            )
                            || (co.Controls[j].Top <= co.Controls[k].Top
                            && co.Controls[j].Bottom >= co.Controls[k].Bottom

                            )
                            ))
                    {
                        //co.Controls[k].Width = co.Controls[j].Left - co.Controls[k].Left - 5;

                        if (co.Controls[k] is CheckBox)
                        {
                            CheckBox chk = co.Controls[k] as CheckBox;

                            chk.AutoSize = false;
                            chk.Left = left;
                            chk.Top = top;
                            chk.Width = co.Controls[j].Left - co.Controls[k].Left - 5;
                            chk.Height = height;
                            chk.AutoEllipsis = true;
                        }
                        else if (co.Controls[k] is Label)
                        {
                            Label chk = co.Controls[k] as Label;

                            chk.AutoSize = false;
                            chk.Left = left;
                            chk.Top = top;
                            chk.Height = height;
                            chk.Width = co.Controls[j].Left - co.Controls[k].Left - 5;
                            chk.AutoEllipsis = true;

                        }
                        else if (co.Controls[k] is RadioButton)
                        {
                            RadioButton chk = co.Controls[k] as RadioButton;

                            chk.AutoSize = false;
                            chk.Left = left;
                            chk.Top = top;
                            chk.Width = co.Controls[j].Left - co.Controls[k].Left - 5;
                            chk.Height = height;
                            chk.AutoEllipsis = true;
                        }

                        tooltip.SetToolTip(co.Controls[k], co.Controls[k].Text);

                        break;
                    }
                }
            }
        }
    }
}
