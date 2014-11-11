//
//  VoltorbFlipGridDllWrapper.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPGRIDDLLWRAPPER_H__
#define __VOLTORBFLIPGRIDDLLWRAPPER_H__

#include "VoltorbFlipProbabilities.h"

public ref class VoltorbFlipGridDllWrapper
{
public:
    static array< array< int >^, 2 >^ solve( array< System::Tuple< int, int >^ >^ rowClues, array< System::Tuple< int, int >^ >^ columnClues, array< int, 2 >^ gridValues );

    static int getGridSize() { return GridSize; }
    static int getNumberOfPossibleValues() { return NumberOfPossibleValues; }
    static int getCouldBeZeroOrOneValue( ) { return VoltorbFlipGrid::CouldBeZeroOrOneValue; }
    static int getUnknownValue() { return VoltorbFlipGrid::UnknownValue; }

private:
    static VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipClue > convertCluesToNative( array< System::Tuple< int, int >^ >^ lineClues );
};

#endif // __VOLTORBFLIPGRIDDLLWRAPPER_H__