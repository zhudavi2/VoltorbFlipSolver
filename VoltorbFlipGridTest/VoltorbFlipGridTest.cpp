//
//  VoltorbFlipTest.cpp
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "../VoltorbFlipGridSolver/VoltorbFlipGridSolver.h"
#include <iostream>

int main()
{
    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGridSolver::VoltorbFlipClue > rowClues = 
    {
        { { 6, 3 }, { 9, 0 }, { 4, 3 }, { 8, 1 }, { 4, 3 } }
    };

    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGridSolver::VoltorbFlipClue > columnClues =
    {
        { { 8, 1 }, { 8, 1 }, { 2, 3 }, { 6, 3 }, { 7, 2 } }
    };

    VoltorbFlipGridSolver gridSolver( rowClues, columnClues );

    gridSolver.solve( );

    system( "pause" );
}