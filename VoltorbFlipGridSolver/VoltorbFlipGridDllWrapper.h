//
//  VoltorbFlipGridDllWrapper.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPGRIDDLLWRAPPER_H__
#define __VOLTORBFLIPGRIDDLLWRAPPER_H__

#include "VoltorbFlipGridSolver.h"

public ref class VoltorbFlipGridDllWrapper
{
public:
    enum class VoltorbFlipValueManaged
    {
        VFVMZero = VoltorbFlipGrid::VoltorbFlipValue::VFVZero,
        VFVMOne = VoltorbFlipGrid::VoltorbFlipValue::VFVOne,
        VFVMTwo = VoltorbFlipGrid::VoltorbFlipValue::VFVTwo,
        VFVMThree = VoltorbFlipGrid::VoltorbFlipValue::VFVThree,
        VFVMZeroOrOne = VoltorbFlipGrid::VoltorbFlipValue::VFVZeroOrOne,
        VFVMUnknown = VoltorbFlipGrid::VoltorbFlipValue::VFVUnknown,
    };

    static const int GridSize = VoltorbFlipGridSize;
    static const int NumberOfValues = VoltorbFlipGridSolver::NumberOfValues;
    static const int MaximumIntValueForZeroOrOne = 9;

    static array< System::Collections::Generic::IDictionary< VoltorbFlipValueManaged, int >^, 2 >^ solveGridWithIntValues( array< System::Tuple< int, int >^ >^ rowClues, array< System::Tuple< int, int >^ >^ columnClues, array< int, 2 >^ gridIntValues );

private:
    static VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGridSolver::VoltorbFlipClue > convertCluesToNative( array< System::Tuple< int, int >^ >^ lineClues );
};

#endif // __VOLTORBFLIPGRIDDLLWRAPPER_H__