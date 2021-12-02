/**
    \file   CountObjects.cs
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
    public class CountObjects {

        private int externalCount = 0;  //<<< count of external corners
        private int internalCount = 0;  //<<< count of internal corners
        private int objectCount   = 0;  //<<< count of objects

        private bool isBinary     = false;
        private bool isBorderOK   = false;
        private bool is4Connected = false;

        public void visit ( GrayImageData g ) {

            int row=g.getH();
            int col = g.getW();
            CheckBinaryVisitor  cb=new  CheckBinaryVisitor();
            CheckBorderEmptyVisitor cb1=new CheckBorderEmptyVisitor();
            Check4ConnectedVisitor  ck=new  Check4ConnectedVisitor();
            cb.visit( g );
            cb1.visit( g );
            ck.visit( g );
            int min=cb.getMin();
            int max=cb.getMax();
            isBinary = cb.get();
            isBorderOK = cb1.get();
            is4Connected = ck.get();
            if ((cb.get() == true)  && (cb1.get()==true)  &&  (ck.getBadCount()==0) &&  (ck.get()==true) )    //check image is binary ,empty border and fully 4 connected
                {
                for(int r=0;r<row-1;r++) 
                    {
                        for (int c = 0; c < col - 1; c++) 
                        {
                            if ((g.getGray( r, c ) == min) && (g.getGray( r + 1, c ) == min) && (g.getGray( r, c + 1 ) == min) && (g.getGray( r + 1, c + 1 ) == max)) //pattern matching
                            {
                            externalCount += 1;
                             } 
                            else if ((g.getGray( r, c ) == min) && (g.getGray( r + 1, c ) == min) && (g.getGray( r, c + 1 ) == max) && (g.getGray( r + 1, c + 1 ) == min))//pattern matching
                            {
                            externalCount += 1;
                            } 
                            else if ((g.getGray( r, c ) == min) && (g.getGray( r + 1, c ) == max) && (g.getGray( r, c + 1 ) == min) && (g.getGray( r + 1, c + 1 ) == min))//pattern matching
                            {
                            externalCount += 1;
                            } 
                            else if ((g.getGray( r, c ) == max) && (g.getGray( r + 1, c ) == min) && (g.getGray( r, c + 1 ) == min) && (g.getGray( r + 1, c + 1 ) == min))//pattern matching
                            {
                            externalCount += 1;
                            } 
                            else if ((g.getGray( r, c ) == max) && (g.getGray( r + 1, c ) == max) && (g.getGray( r, c + 1 ) == max) && (g.getGray( r + 1, c + 1 ) == min))//pattern matching
                            {
                            internalCount += 1;
                            }
                            else if ((g.getGray( r, c ) == max) && (g.getGray( r + 1, c ) == max) && (g.getGray( r, c + 1 ) == min) && (g.getGray( r + 1, c + 1 ) == max)) //pattern matching
                            {
                            internalCount += 1;
                            } 
                            else if ((g.getGray( r, c ) == max) && (g.getGray( r + 1, c ) == min) && (g.getGray( r, c + 1 ) == max) && (g.getGray( r + 1, c + 1 ) == max))//pattern matching
                            {
                            internalCount += 1;
                            } 
                            else if ((g.getGray( r, c ) == min) && (g.getGray( r + 1, c ) == max) && (g.getGray( r, c + 1 ) == max) && (g.getGray( r + 1, c + 1 ) == max)) //pattern matching
                            {
                            internalCount += 1;
                            }

                        }
                       
                    }
                objectCount = (externalCount - internalCount) / 4;  //object count
            }
            
        }
        public int get ( ) { return objectCount; }
        public int getInternal ( ) { return internalCount; }
        public int getExternal ( ) { return externalCount; }

        public bool getIsBinary ( ) { return isBinary; }
        public bool getIsBorderOK ( ) { return isBorderOK; }
        public bool getIs4Connected ( ) { return is4Connected; }
    }
}
