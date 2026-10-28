import { act, fireEvent, render, screen } from '@testing-library/react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { FeatureTools } from './FeatureTools'
import { useMapStore } from '../store/mapStore'

describe('FeatureTools', () => {
  beforeEach(() => {
    useMapStore.setState({
      activeTool: 'none',
      polygonDraft: null,
      objectDraft: null,
      objectName: 'New object',
      objectSymbolType: 'marker',
      selection: null,
    })
  })

  it('creates a closed GeoJSON polygon with longitude-latitude coordinates', () => {
    const onCreatePolygon = vi.fn()

    render(
      <FeatureTools
        objectCount={0}
        polygonCount={0}
        onCreateObject={vi.fn()}
        onCreatePolygon={onCreatePolygon}
        objectSaving={false}
        polygonSaving={false}
      />,
    )

    fireEvent.click(screen.getByRole('button', { name: /add polygon/i }))
    const addVertex = useMapStore.getState().addPolygonVertex
    act(() => {
      addVertex({ lat: 32, lng: 34 } as L.LatLng)
      addVertex({ lat: 33, lng: 35 } as L.LatLng)
      addVertex({ lat: 31, lng: 36 } as L.LatLng)
    })

    fireEvent.click(screen.getByRole('button', { name: /finish polygon/i }))
    fireEvent.click(screen.getByRole('button', { name: /save polygon/i }))

    expect(onCreatePolygon).toHaveBeenCalledWith({
      name: 'New polygon',
      geometry: {
        type: 'Polygon',
        coordinates: [[[34, 32], [35, 33], [36, 31], [34, 32]]],
      },
    })
  })

  it('saves a placed object as a GeoJSON point', () => {
    const onCreateObject = vi.fn()

    render(
      <FeatureTools
        objectCount={0}
        polygonCount={0}
        onCreateObject={onCreateObject}
        onCreatePolygon={vi.fn()}
        objectSaving={false}
        polygonSaving={false}
      />,
    )

    fireEvent.change(screen.getAllByLabelText('Object name')[0]!, {
      target: { value: 'Jeep Alpha' },
    })
    fireEvent.change(screen.getByLabelText('Symbol'), {
      target: { value: 'jeep' },
    })
    fireEvent.click(screen.getByRole('button', { name: /place object/i }))
    act(() => {
      useMapStore.getState().placeObject({ lat: 32.0853, lng: 34.7818 } as L.LatLng)
    })
    fireEvent.click(screen.getByRole('button', { name: /save object/i }))

    expect(onCreateObject).toHaveBeenCalledWith({
      name: 'Jeep Alpha',
      symbolType: 'jeep',
      geometry: { type: 'Point', coordinates: [34.7818, 32.0853] },
    })
  })
})
