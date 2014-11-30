//
//  VoltorbFlipGrid.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPGRID_H__
#define __VOLTORBFLIPGRID_H__

#include <array>
#include <map>

// Causes duplicate definition issues if declared inside the class
// Must be declared in the .h so that VoltorbFlipLine may be declared
static const int VoltorbFlipGridSize = 5;

class VoltorbFlipGrid
{
public:
    enum VoltorbFlipValue
    {
        VFVZero = 0,
        VFVOne,
        VFVTwo,
        VFVThree,
        VFVZeroOrOne,
        VFVUnknown
    };

    template< class T >
    using VoltorbFlipLine = std::array< T, VoltorbFlipGridSize >;

    VoltorbFlipGrid();

    inline VoltorbFlipValue getEntryValue( int rowIndex, int columnIndex ) const
    { return m_gridValues.at( rowIndex ).at( columnIndex ); }

    inline void setEntryValue( int rowIndex, int columnIndex, VoltorbFlipValue value = VoltorbFlipValue::VFVUnknown )
    { m_gridValues.at( rowIndex ).at( columnIndex ) = value; }

    VoltorbFlipLine< VoltorbFlipValue > getRow( int row ) const;
    void setRow( int r, const VoltorbFlipLine< VoltorbFlipValue >& row );

    VoltorbFlipLine< VoltorbFlipValue > getColumn( int col ) const;
    void setColumn( int c, const VoltorbFlipLine< VoltorbFlipValue >& column );

private:
    VoltorbFlipLine< VoltorbFlipLine< VoltorbFlipValue > > m_gridValues;
};

#endif // __VOLTORBFLIPGRID_H__
