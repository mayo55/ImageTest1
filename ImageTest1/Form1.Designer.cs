namespace ImageTest1
{
    partial class Form1 : System.Windows.Forms.Form
    {

        //フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Windows フォーム デザイナーで必要です。

        private System.ComponentModel.IContainer components;
        //メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
        //Windows フォーム デザイナーを使用して変更できます。  
        //コード エディターを使って変更しないでください。
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.FileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(284, 262);
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // FileSystemWatcher1
            // 
            this.FileSystemWatcher1.EnableRaisingEvents = true;
            this.FileSystemWatcher1.SynchronizingObject = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.IO.FileSystemWatcher FileSystemWatcher1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
