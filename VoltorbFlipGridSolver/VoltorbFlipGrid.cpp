//
//  VoltorbFlipGrid.cpp
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "VoltorbFlipGrid.h"

VoltorbFlipGrid::VoltorbFlipGrid()
{
    for ( auto& row : m_gridValues )
    {
        for ( auto& value : row )
        {
            value = VoltorbFlipValue::VFVUnknown;
        }
    }
}

VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue > VoltorbFlipGrid::getRow( int r ) const
{
    return m_gridValues.at( r );
}

void VoltorbFlipGrid::setRow( int r, const VoltorbFlipLine< VoltorbFlipValue >& row )
{
    m_gridValues.at( r ) = row;
}

VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue > VoltorbFlipGrid::getColumn( int c ) const
{
    VoltorbFlipLine< VoltorbFlipValue > column;
    for ( int r=0; r<VoltorbFlipGridSize; r++ )
    {
        column.at( r ) = m_gridValues.at( r ).at( c );
    }
    return column;
}

void VoltorbFlipGrid::setColumn( int c, const VoltorbFlipLine< VoltorbFlipValue >& column )
{
    for ( int r=0; r<VoltorbFlipGridSize; r++ )
    {
        m_gridValues.at( r ).at( c ) = column.at( r );
    }
}
