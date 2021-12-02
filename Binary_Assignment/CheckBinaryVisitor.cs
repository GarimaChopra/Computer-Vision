/**
    \file   CheckBinaryVisitor.cs
    \brief  Contains Functions definition.
    \author Garima Chopra

 */
//----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//----------------------------------------------------------------------
namespace CSImageViewer {
    public class CheckBinaryVisitor {
        private bool isBinary = false;
        private int  min      = 0;
        private int  max      = 0;

 	/** \brief  <b> This method checks if image is binary image</b>
         *  
         *  \param  g        object of GrayImageData
         * 
         *
         *
         *  \returns  sets value of isBinary,min,max
         */

        public void visit ( GrayImageData g ) {

            int size=g.getW()*g.getH();     //size of array for gray image

            for (int i = 0; i < size; i++) {
                if (g.getData( i ) < min) min = g.getData( i ); //if value less than min swap
                if (g.getData( i ) > max) max = g.getData( i ); //if value greater than max swap
            }
            for (int i = 0; i < size; i++) {
                if ((g.getData( i ) == min) || (g.getData( i ) == max)) { //if value is equal to max or min image is binary
                    isBinary = true;

                } else {
                    isBinary = false;
                    break;
                }
            }
        
        }

        public bool get ( ) { return isBinary; }
        public int getMin ( ) { return min; }
        public int getMax ( ) { return max; }
    }
}
