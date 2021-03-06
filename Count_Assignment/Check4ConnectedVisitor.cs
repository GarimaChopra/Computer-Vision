/**
    \file   Check4Connected.cs
    \brief  Contains Functions definition.
    \author Garima Chopra
 */
//----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
//----------------------------------------------------------------------
namespace CSImageViewer {
    public class Check4ConnectedVisitor {
        private bool is4Connected = false;
        private int  badCount     = 0;

        //record the position of one bad one (if any).
        // it does not matter which one (if any).
        private int  badRow       = -1;
        private int  badCol       = -1;

        /** \brief  <b> This method checks if input image is 4connected
             *  
             *  \param  g        GrayImageData class object
             *
             *
             *  \returns   sets value of badRow,badCount,badCol,is4Connected       
         */


        public void visit ( GrayImageData g ) {
            int row=g.getH();
            int col = g.getW();
            int[,]  L=new   int[row,col];   //array for label
            int num=1;
            CheckBinaryVisitor  cb=new  CheckBinaryVisitor();
            CheckBorderEmptyVisitor cb1=new CheckBorderEmptyVisitor();
            cb.visit( g );
            cb1.visit( g );
            int min=cb.getMin();
            int max=cb.getMax();
            if (max == min)
                is4Connected = true;
            else 
            
            {
                if (cb.get() == true && cb1.get() == true) {       //if image is Binary and border empty
                    for (int r = 1; r < row; r++) {
                        for (int c = 1; c < col; c++) {
                            if (g.getGray( r, c ) == min) {     //if value is min then label is 0 else assign new label
                                L[ r, c ] = 0;
                                if (g.getGray( r - 1, c - 1 ) == min && g.getGray( r - 1, c ) == max && g.getGray( r, c - 1 ) == max)
                                    {
                                    badRow = r;
                                    badCol = c;
                                    badCount = badCount + 1;
                                     }
                            }
                            else {
                                if (g.getGray( r - 1, c - 1 ) == max && g.getGray( r - 1, c ) == min && g.getGray( r, c - 1 ) == min) {
                                    badRow = r;
                                    badCol = c;
                                    badCount = badCount + 1;

                                }
                                if (L[ r - 1, c ] == 0 && L[ r, c - 1 ] == 0) {   //Check label for left and top column
                                    L[ r, c ] = num++;
                                } 
                                else if (L[ r - 1, c ] != 0 && L[ r, c - 1 ] == 0) //if top col has label  use same label
                                      {
                                    L[ r, c ] = L[ r - 1, c ];
                                    is4Connected = true;
                                } else if (L[ r - 1, c ] == 0 && L[ r, c - 1 ] != 0)  //if left col has label  use same label
                                      {
                                    L[ r, c ] = L[ r, c - 1 ];
                                    is4Connected = true;
                                } else if (L[ r - 1, c ] != 0 && L[ r, c - 1 ] != 0) //if both have label use min label
                                      {
                                    if (L[ r - 1, c ] < L[ r, c - 1 ]) {
                                        L[ r, c - 1 ] = L[ r - 1, c ];
                                        L[ r, c ] = L[ r - 1, c ];
                                        is4Connected = true;
                                    } else if (L[ r, c - 1 ] < L[ r - 1, c ]) {
                                        L[ r - 1, c ] = L[ r, c - 1 ];
                                        L[ r, c ] = L[ r, c - 1 ];
                                        is4Connected = true;
                                    } else {
                                        L[ r, c - 1 ] = L[ r - 1, c ] = L[ r, c ];
                                        is4Connected = true;
                                    }
                                }
                            }
                        }

                    }

                } else if (cb.get() == true && cb1.get() != true)  //if border is not empty mark label for non empty border positions
                   {
                    int h=g.getH();
                    int w = g.getW();
                    int l=1;
                    for (int r = 0; r < g.getW(); r++) {    //Assign labels to top row if pixel not empty
                        if (g.getGray( 0, r ) != min)
                            L[ 0, r ] = l++;
                        else {
                            L[ 0, r ] = 0;
                          
                        }

                    }

                    for (int r = 1; r < g.getH(); r++) {      //Assign labels to left column if pixel not empty
                        if (g.getGray( r, 0 ) != min)
                            L[ r, 0 ] = l++;
                        else {
                            L[ r, 0 ] = 0;
                            
                        }

                    }

                    //code to check 4 connected in  border(top  row &  left col)

                    if (L[ 0, 0 ] != 0) {            //Check first col label
                        if (L[ 1, 0 ] != 0) {         //Check second row first col
                            L[ 1, 0 ] = L[ 0, 0 ];
                            is4Connected = true;
                        } else if (L[ 0, 1 ] != 0) {  //Check first row second col
                            L[ 0, 1 ] = L[ 0, 0 ];
                            is4Connected = true;
                        } else if (L[ 0, 1 ] != 0 && L[ 1, 0 ] != 0) {   //if  both col below and col to right are connected  
                            L[ 1, 0 ] = L[ 0, 1 ] = L[ 0, 0 ];
                            is4Connected = true;
                        }
                    }


                    if (g.getGray( 0, g.getW() - 1 ) != min) {       //Check last col of first  row
                        if (g.getGray( 1, g.getW() - 1 ) != min) {   //check 2nd row last  col
                            L[ 1, g.getW() - 1 ] = L[ 0, g.getW() - 1 ];
                            is4Connected = true;
                        }
                    }
                    for (int r = 1; r < g.getW() - 1; r++) {    //Check top row for other than corners
                        if (g.getGray( 0, r ) != 0) {
                            if (g.getGray( 0, r + 1 ) != 0) {
                                is4Connected = true;
                            }
                        }
                    }
                    if (g.getGray( h - 1, 0 ) != min) {    //Check last row  first  col
                        if (g.getGray( h - 2, 0 ) != min) {
                            L[ h - 2, 0 ] = L[ h - 1, 0 ];
                            is4Connected = true;
                        }
                    }

                    for (int r = 1; r < g.getH() - 1; r++) {    //Check left col for other than corners
                        if (g.getGray( r, 0 ) != 0) {
                            if (g.getGray( r + 1, 0 ) != 0) {
                                is4Connected = true;
                            }
                        }

                    }

                    //All other columns
                    for (int r = 1; r < row; r++) {
                        for (int c = 1; c < col; c++) {
                            if (g.getGray( r, c ) == min) {     //if value is min then label is 0
                                L[ r, c ] = 0;
                                if (g.getGray( r - 1, c - 1 ) == min && g.getGray( r - 1, c ) == max && g.getGray( r, c - 1 ) == max) {
                                    badRow = r;
                                    badCol = c;
                                    badCount = badCount + 1;
                                }
                            } else {
                                if (g.getGray( r - 1, c - 1 ) == max && g.getGray( r - 1, c ) == min && g.getGray( r, c - 1 ) == min) {
                                    badRow = r;
                                    badCol = c;
                                    badCount = badCount + 1;

                                }
                                if (L[ r - 1, c ] == 0 && L[ r, c - 1 ] == 0) {   //Check label for left and top column
                                    L[ r, c ] = l++;
                                } else if (L[ r - 1, c ] != 0 && L[ r, c - 1 ] == 0) //if top col has label  use same label
                                      {
                                    L[ r, c ] = L[ r - 1, c ];
                                    is4Connected = true;
                                } else if (L[ r - 1, c ] == 0 && L[ r, c - 1 ] != 0)  //if left col has label  use same label
                                      {
                                    L[ r, c ] = L[ r, c - 1 ];
                                    is4Connected = true;
                                } else if (L[ r - 1, c ] != 0 && L[ r, c - 1 ] != 0) //if both have label use min label
                                      {
                                    if (L[ r - 1, c ] < L[ r, c - 1 ]) {
                                        L[ r, c - 1 ] = L[ r - 1, c ];
                                        L[ r, c ] = L[ r - 1, c ];
                                        is4Connected = true;
                                    } else if (L[ r, c - 1 ] < L[ r - 1, c ]) {
                                        L[ r - 1, c ] = L[ r, c - 1 ];
                                        L[ r, c ] = L[ r, c - 1 ];
                                        is4Connected = true;
                                    } else {
                                        L[ r, c - 1 ] = L[ r - 1, c ] = L[ r, c ];
                                        is4Connected = true;
                                    }
                                }
                            }
                        }

                    }

                }

            }
        }
        public bool get ( ) { return is4Connected; }
        public int getBadRow ( ) { return badRow; }
        public int getBadCol ( ) { return badCol; }
        public int getBadCount ( ) { return badCount; }
    }
}
