import {useQuery} from "@tanstack/react-query";
import {getAlbumMetadataById, getAlbumMetadataByQuery} from "../api/metadata.ts";

export function useAlbumMetadataByQuery(query?: string) {
    return useQuery({
        queryKey: ["albumMetadataByQuery", query],
        queryFn: () => getAlbumMetadataByQuery(query!),
        enabled: !!query,
    });
}

export function useAlbumMetadataById(id?: string) {
    return useQuery({
        queryKey: ["albumMetadataById", id],
        queryFn: () => getAlbumMetadataById(id!),
        enabled: !!id,
    });
}
