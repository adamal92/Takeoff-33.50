import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { mapApi } from '../services/api'
import { useMapStore } from '../store/mapStore'
import type { CreateMapObject, CreateMapPolygon, FeatureSelection } from '../types'

export const mapQueryKeys = {
  polygons: ['polygons'] as const,
  objects: ['objects'] as const,
}

export function useMapData() {
  const queryClient = useQueryClient()
  const selectFeature = useMapStore((state) => state.selectFeature)
  const discardPolygon = useMapStore((state) => state.discardPolygon)
  const discardObject = useMapStore((state) => state.discardObject)

  const polygonsQuery = useQuery({
    queryKey: mapQueryKeys.polygons,
    queryFn: mapApi.getPolygons,
  })
  const objectsQuery = useQuery({
    queryKey: mapQueryKeys.objects,
    queryFn: mapApi.getObjects,
  })

  const createPolygon = useMutation({
    mutationFn: (polygon: CreateMapPolygon) => mapApi.createPolygon(polygon),
    onSuccess: async (polygon) => {
      discardPolygon()
      selectFeature({ type: 'polygon', id: polygon.id })
      await queryClient.invalidateQueries({ queryKey: mapQueryKeys.polygons })
    },
  })

  const createObject = useMutation({
    mutationFn: (mapObject: CreateMapObject) => mapApi.createObject(mapObject),
    onSuccess: async (mapObject) => {
      discardObject()
      selectFeature({ type: 'object', id: mapObject.id })
      await queryClient.invalidateQueries({ queryKey: mapQueryKeys.objects })
    },
  })

  const deleteFeature = useMutation({
    mutationFn: async ({ type, id }: FeatureSelection) => {
      if (type === 'polygon') return mapApi.deletePolygon(id)
      return mapApi.deleteObject(id)
    },
    onSuccess: async (_, feature) => {
      selectFeature(null)
      await queryClient.invalidateQueries({
        queryKey: feature.type === 'polygon' ? mapQueryKeys.polygons : mapQueryKeys.objects,
      })
    },
  })

  return {
    polygons: polygonsQuery.data ?? [],
    objects: objectsQuery.data ?? [],
    isLoading: polygonsQuery.isLoading || objectsQuery.isLoading,
    errors: [polygonsQuery.error, objectsQuery.error].filter(
      (error): error is Error => error instanceof Error,
    ),
    reload: () => Promise.all([polygonsQuery.refetch(), objectsQuery.refetch()]),
    createPolygon,
    createObject,
    deleteFeature,
  }
}
