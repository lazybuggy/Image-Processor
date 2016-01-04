using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//Lucia Okeh
//Image Processing
//October 28, 2013
//Write code that changes the values of a picture in various ways including.. Mirroring, Rotating, Flipping
// resetign back to original , and bluring.

namespace ImageProcessing
{
    public partial class frmMain : Form
    {
        private Color[,] original; //this is the original picture - never change the values stored in this array
        private Color[,] transformedPic;  //transformed picture that is displayed
       

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //this method draws the transformed picture
            //what ever is stored in transformedPic array will
            //be displayed on the form

            base.OnPaint(e);

            Graphics g = e.Graphics;

            //only draw if picture is transformed
            if (transformedPic != null)
            {
                //get height and width of the transfrormedPic array
                int height = transformedPic.GetUpperBound(0)+1;
                int width = transformedPic.GetUpperBound(1) + 1;

                //create a new Bitmap to be dispalyed on the form
                Bitmap newBmp = new Bitmap(width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //loop through each element transformedPic and set the 
                        //colour of each pixel in the bitmalp
                        newBmp.SetPixel(j, i, transformedPic[i, j]);
                    }

                }
                //call DrawImage to draw the bitmap
                g.DrawImage(newBmp, 0, 20, width, height);
            }
            
        }


        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //this method reads in a picture file and stores it in an array

            //try catch should handle any errors for invalid picture files
            try
            {

                //open the file dialog to select a picture file

                OpenFileDialog fd = new OpenFileDialog();

                //create a bitmap to store the file in
                Bitmap bmp;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //store the selected file into a bitmap
                    bmp = new Bitmap(fd.FileName);

                    //create the arrays that store the colours for the image
                    //the size of the arrays is based on the height and width of the bitmap
                    //initially both the original and transformedPic arrays will be identical
                    original = new Color[bmp.Height, bmp.Width];
                    transformedPic = new Color[bmp.Height, bmp.Width];

                    //load each color into a color array
                    for (int i = 0; i < bmp.Height; i++)//each row
                    {
                        for (int j = 0; j < bmp.Width; j++)//each column
                        {
                            //assign the colour in the bitmap to the array
                            original[i, j] = bmp.GetPixel(j, i);
                            transformedPic[i, j] = original[i, j];
                        }
                    }
                    //this will cause the form to be redrawn and OnPaint() will be called
                    this.Refresh();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Picture File. \n" + ex.Message);
            }
            
        }

        private void mnuProcessDarken_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to Darken        
                int Red, Green, Blue;

                //transformed pic at this point is = to the original pic
                if (transformedPic != null)
                {
                    //rows
                    int Height = transformedPic.GetLength(0);
                    //columns
                    int Width = transformedPic.GetLength(1);

                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Red = transformedPic[i, j].R - 10;
                            Blue = transformedPic[i, j].B - 10;
                            Green = transformedPic[i, j].G - 10;

                            //set minimum amount
                            if (Red < 0 || Green < 0 || Blue < 0)
                            {
                                Red = 0;
                                Green = 0;
                                Blue = 0;
                            }

                            //put the new colours together using fromARGB
                            transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                        }
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessInvert_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to Invert
                int Red, Green, Blue;

                //transformed pic at this point is = to the original pic
                if (transformedPic != null)
                {
                    //rows
                    int Height = transformedPic.GetLength(0);
                    //columns
                    int Width = transformedPic.GetLength(1);

                    // loop through each element in transfored pic
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Red = 255 - transformedPic[i, j].R;
                            Blue = 255 - transformedPic[i, j].B;
                            Green = 255 - transformedPic[i, j].G;

                            //put the new colours together using fromARGB
                            transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                        }
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessWhiten_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to Whiten

                int Red, Green, Blue;

                //transformed pic at this point is = to the original pic
                if (transformedPic != null)
                {
                    //rows
                    int Height = transformedPic.GetLength(0);
                    //columns
                    int Width = transformedPic.GetLength(1);

                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Red = transformedPic[i, j].R + 10;
                            Blue = transformedPic[i, j].B + 10;
                            Green = transformedPic[i, j].G + 10;

                            //set maximum amount
                            if (Red > 255 || Green > 255 || Blue > 255)
                            {
                                Red = 255;
                                Green = 255;
                                Blue = 255;
                            }

                            //put the new colours together using fromARGB
                            transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                        }
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessRotate_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to Rotate

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //create copy array
                Color[,] copyPic = new Color[Height, Width];

                //populate copy array
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }

                //switch dimensions around
                transformedPic = new Color[Width, Height];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[j, i] = copyPic[i, j];
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessScale50_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to scale 50

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //set new width and height
                int NewHeight = Height / 2;
                int NewWidth = Width / 2;

                //copy the array
                Color[,] copyPic = new Color[Height, Width];

                //populate copy array
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }

                //set new dimensions
                transformedPic = new Color[NewHeight, NewWidth];

                //loop over every other index to get scaled image
                for (int i = 0; i < Height / 2; i++)
                {
                    for (int j = 0; j < Width / 2; j++)
                    {
                        transformedPic[i, j] = copyPic[i * 2, j * 2];
                    }
                }

                this.Refresh();
            }
        }



        private void mnuProcessFlipX_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //create new array
                Color[,] copyPic = new Color[Height, Width];

                //loop amount
                int NewCol = Width / 2;
                
                //populate copyarray
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }

                //code to flip image
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < NewCol; j++)
                    {
                        transformedPic[i, NewCol - j] = copyPic[i, NewCol + j];
                        transformedPic[i, NewCol + j] = copyPic[i, NewCol - j];
                    }
                }


                this.Refresh();
            }
        
        }

        private void mnuProcessFlipY_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to flip vertically

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //creat a copy array
                Color[,] copyPic = new Color[Height, Width];

                //loops
                int NewHeight = Height / 2;

                //populate copy array
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }

                //code to flip image
                for (int i = 0; i < NewHeight; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[NewHeight - i, j] = copyPic[NewHeight + i, j];
                        transformedPic[NewHeight + i, j] = copyPic[NewHeight - i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessMirrorH_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to Mirror image horizontally

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //new width
                int NewWidth = Width * 2;
                //loops for flipcode
                int NewCol = Width / 2;

                //create 2 copy arrays
                Color[,] copyPic = new Color[Height, Width];
                Color[,] copyPic2 = new Color[Height, Width];

                //populate the copy arrays
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                        copyPic2[i, j] = transformedPic[i, j];
                    }
                }

                //flip image horizontally
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < NewCol; j++)
                    {
                        copyPic2[i, NewCol - j] = copyPic[i, NewCol + j];
                        copyPic2[i, NewCol + j] = copyPic[i, NewCol - j];
                    }
                }

                //set new dimensions
                transformedPic = new Color[Height, NewWidth];

                //display the new mirrored image
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[i, j] = copyPic[i, j];
                        transformedPic[i, Width + j] = copyPic2[i, j];
                    }
                }


                this.Refresh();
            }
        }

        private void mnuProcessBlur_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to blur image

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                int AvgRed, AvgGreen, AvgBlue;

                //copy the array
                Color[,] copyPic = new Color[Height, Width];

                //populate copy array
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }

                //code for the middle
                for (int i = 1; i < Height - 1; i++)
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        AvgRed = ((copyPic[i - 1, j - 1].R + copyPic[i - 1, j].R + copyPic[i - 1, j + 1].R + copyPic[i, j - 1].R + copyPic[i, j + 1].R + copyPic[i + 1, j - 1].R + copyPic[i + 1, j].R + copyPic[i + 1, j + 1].R + copyPic[i, j].R) / 9);
                        AvgGreen = ((copyPic[i - 1, j - 1].G + copyPic[i - 1, j].G + copyPic[i - 1, j + 1].G + copyPic[i, j - 1].G + copyPic[i, j + 1].G + copyPic[i + 1, j - 1].G + copyPic[i + 1, j].G + copyPic[i + 1, j + 1].G + copyPic[i, j].G) / 9);
                        AvgBlue = ((copyPic[i - 1, j - 1].B + copyPic[i - 1, j].B + copyPic[i - 1, j + 1].B + copyPic[i, j - 1].B + copyPic[i, j + 1].B + copyPic[i + 1, j - 1].B + copyPic[i + 1, j].B + copyPic[i + 1, j + 1].B + copyPic[i, j].B) / 9);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for top row
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        AvgRed = ((copyPic[i, j - 1].R + copyPic[i + 1, j - 1].R + copyPic[i + 1, j].R + copyPic[i + 1, j + 1].R + copyPic[i, j + 1].R + copyPic[i, j].R) / 6);
                        AvgGreen = ((copyPic[i, j - 1].G + copyPic[i + 1, j - 1].G + copyPic[i + 1, j].G + copyPic[i + 1, j + 1].G + copyPic[i, j + 1].G + copyPic[i, j].G) / 6);
                        AvgBlue = ((copyPic[i, j - 1].B + copyPic[i + 1, j - 1].B + copyPic[i + 1, j].B + copyPic[i + 1, j + 1].B + copyPic[i, j + 1].B + copyPic[i, j].B) / 6);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }
                //code for bottom row
                for (int i = Height - 1; i < 1; i++)
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        AvgRed = ((copyPic[i - 1, j - 1].R + copyPic[i - 1, j + 1].R + copyPic[i - 1, j].R + copyPic[i, j + 1].R + copyPic[i, j - 1].R + copyPic[i, j].R) / 6);
                        AvgGreen = ((copyPic[i - 1, j - 1].G + copyPic[i - 1, j + 1].G + copyPic[i - 1, j].G + copyPic[i, j + 1].G + copyPic[i, j - 1].G + copyPic[i, j].G) / 6);
                        AvgBlue = ((copyPic[i - 1, j - 1].B + copyPic[i - 1, j + 1].B + copyPic[i - 1, j].B + copyPic[i, j + 1].B + copyPic[i, j - 1].B + copyPic[i, j].B) / 6);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for far right column
                for (int i = 1; i < Height - 1; i++)
                {
                    for (int j = Width - 1; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i - 1, j].R + copyPic[i - 1, j - 1].R + copyPic[i, j - 1].R + copyPic[i + 1, j - 1].R + copyPic[i + 1, j].R) / 6);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i - 1, j].G + copyPic[i - 1, j - 1].G + copyPic[i, j - 1].G + copyPic[i + 1, j - 1].G + copyPic[i + 1, j].G) / 6);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i - 1, j].B + copyPic[i - 1, j - 1].B + copyPic[i, j - 1].B + copyPic[i + 1, j - 1].B + copyPic[i + 1, j].B) / 6);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for far left column
                for (int i = 1; i < Height - 1; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i - 1, j].R + copyPic[i - 1, j + 1].R + copyPic[i, j + 1].R + copyPic[i + 1, j].R + copyPic[i + 1, j + 1].R) / 6);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i - 1, j].G + copyPic[i - 1, j + 1].G + copyPic[i, j + 1].G + copyPic[i + 1, j].G + copyPic[i + 1, j + 1].G) / 6);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i - 1, j].B + copyPic[i - 1, j + 1].B + copyPic[i, j + 1].B + copyPic[i + 1, j].B + copyPic[i + 1, j + 1].B) / 6);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for top left corner
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i + 1, j].R + copyPic[i, j + 1].R + copyPic[i + 1, j + 1].R) / 4);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i + 1, j].G + copyPic[i, j + 1].G + copyPic[i + 1, j + 1].G) / 4);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i + 1, j].B + copyPic[i, j + 1].B + copyPic[i + 1, j + 1].B) / 4);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for top right corner
                for (int i = 0; i < 1; i++)
                {
                    for (int j = Width - 1; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i + 1, j].R + copyPic[i, j - 1].R + copyPic[i + 1, j - 1].R) / 4);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i + 1, j].G + copyPic[i, j - 1].G + copyPic[i + 1, j - 1].G) / 4);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i + 1, j].B + copyPic[i, j - 1].B + copyPic[i + 1, j - 1].B) / 4);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for bottom left corner
                for (int i = Height - 1; i < 1; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i - 1, j].R + copyPic[i, j + 1].R + copyPic[i - 1, j + 1].R) / 4);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i - 1, j].G + copyPic[i, j + 1].G + copyPic[i - 1, j + 1].G) / 4);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i - 1, j].B + copyPic[i, j + 1].B + copyPic[i - 1, j + 1].B) / 4);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                //code for bottom right corner
                for (int i = Height - 1; i < 1; i++)
                {
                    for (int j = Width - 1; j < 1; j++)
                    {
                        AvgRed = ((copyPic[i, j].R + copyPic[i - 1, j].R + copyPic[i, j - 1].R + copyPic[i - 1, j - 1].R) / 4);
                        AvgGreen = ((copyPic[i, j].G + copyPic[i - 1, j].G + copyPic[i, j - 1].G + copyPic[i - 1, j - 1].G) / 4);
                        AvgBlue = ((copyPic[i, j].B + copyPic[i - 1, j].B + copyPic[i, j - 1].B + copyPic[i - 1, j - 1].B) / 4);

                        transformedPic[i, j] = Color.FromArgb(AvgRed, AvgGreen, AvgBlue);
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessScale200_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to scale 200

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                int NewHeight = Height * 2;
                int NewWidth = Width * 2;

                //copy the array
                Color[,] copyPic = new Color[Height, Width];

                //populate the copy array
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                    }
                }
                //set new dimensions for transformedpic
                transformedPic = new Color[NewHeight, NewWidth];

                //code to double the images size by copying each pixel to the next
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[i * 2, j * 2] = copyPic[i, j];
                        transformedPic[i * 2, j * 2 + 1] = copyPic[i, j];
                        transformedPic[i * 2 + 1, j * 2] = copyPic[i, j];
                        transformedPic[i * 2 + 1, j * 2 + 1] = copyPic[i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessReset_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to reset the picture

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);


                //old row
                int OldHeight = original.GetLength(0);
                //old col
                int OldWidth = original.GetLength(1);

                //set transformedpic back to its original dimensions
                transformedPic = new Color[OldHeight, OldWidth];

                //populate transformedpic with the original pictures values
                for (int i = 0; i < OldHeight; i++)
                {
                    for (int j = 0; j < OldWidth; j++)
                    {
                        transformedPic[i, j] = original[i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessMirrorV_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //code to vertically flip image

                //rows
                int Height = transformedPic.GetLength(0);
                //columns
                int Width = transformedPic.GetLength(1);

                //set new height
                int NewHeight = Height * 2;
                //set loops
                int NewRow = Height / 2;

                // make 2 new arrays
                Color[,] copyPic = new Color[Height, Width];
                Color[,] copyPic2 = new Color[Height, Width];

                //populate the 2 new arrays
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        copyPic[i, j] = transformedPic[i, j];
                        copyPic2[i, j] = transformedPic[i, j];
                    }
                }
                //flip image vertically
                for (int i = 0; i < NewRow; i++)//each row
                {
                    for (int j = 0; j < Width; j++)//each column
                    {
                        copyPic2[NewRow + i, j] = copyPic[NewRow - i, j];
                        copyPic2[NewRow - i, j] = copyPic[NewRow + i, j];
                    }
                }
                //set new dimensions
                transformedPic = new Color[NewHeight, Width];

                //display the mirroed image
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width - 1; j++)
                    {
                        transformedPic[i, j] = copyPic[i, j];
                        transformedPic[Height + i, j] = copyPic2[i, j];
                    }
                }
                this.Refresh();
            }
        }

       
    }
}
