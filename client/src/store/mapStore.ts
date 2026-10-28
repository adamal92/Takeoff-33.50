import { create } from 'zustand'
import type { FeatureSelection, PolygonDraft } from '../types'
import type { LatLng } from 'leaflet'

export type MapTool = 'none' | 'polygon' | 'object'

interface MapState {
  activeTool: MapTool
  polygonDraft: PolygonDraft | null
  objectDraft: { name: string; symbolType: string; position: LatLng } | null
  objectName: string
  objectSymbolType: string
  selection: FeatureSelection | null
  setTool: (tool: MapTool) => void
  addPolygonVertex: (position: LatLng) => void
  finishPolygon: () => void
  setPolygonName: (name: string) => void
  discardPolygon: () => void
  setObjectName: (name: string) => void
  setObjectSymbolType: (symbolType: string) => void
  placeObject: (position: LatLng) => void
  discardObject: () => void
  selectFeature: (selection: FeatureSelection | null) => void
}

export const useMapStore = create<MapState>((set, get) => ({
  activeTool: 'none',
  polygonDraft: null,
  objectDraft: null,
  objectName: 'New object',
  objectSymbolType: 'marker',
  selection: null,
  setTool: (tool) => {
    if (tool === 'polygon' && !get().polygonDraft) {
      set({
        activeTool: tool,
        polygonDraft: { name: 'New polygon', positions: [], complete: false },
        objectDraft: null,
      })
      return
    }

    set({ activeTool: tool, objectDraft: null })
  },
  addPolygonVertex: (position) => {
    const draft = get().polygonDraft
    if (!draft || draft.complete) return
    set({ polygonDraft: { ...draft, positions: [...draft.positions, position] } })
  },
  finishPolygon: () => {
    const draft = get().polygonDraft
    if (!draft || draft.positions.length < 3) return
    set({
      activeTool: 'none',
      polygonDraft: { ...draft, complete: true },
    })
  },
  setPolygonName: (name) => {
    const draft = get().polygonDraft
    if (draft) set({ polygonDraft: { ...draft, name } })
  },
  discardPolygon: () => set({ activeTool: 'none', polygonDraft: null }),
  setObjectName: (objectName) => {
    const draft = get().objectDraft
    set({
      objectName,
      ...(draft ? { objectDraft: { ...draft, name: objectName } } : {}),
    })
  },
  setObjectSymbolType: (objectSymbolType) => {
    const draft = get().objectDraft
    set({
      objectSymbolType,
      ...(draft ? { objectDraft: { ...draft, symbolType: objectSymbolType } } : {}),
    })
  },
  placeObject: (position) => {
    const { objectName, objectSymbolType } = get()
    set({
      activeTool: 'none',
      objectDraft: { name: objectName, symbolType: objectSymbolType, position },
    })
  },
  discardObject: () => set({ activeTool: 'none', objectDraft: null }),
  selectFeature: (selection) => set({ selection }),
}))
