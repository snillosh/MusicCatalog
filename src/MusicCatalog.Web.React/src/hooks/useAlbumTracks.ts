import { useQuery } from "@tanstack/react-query";
import { getAlbumTracks } from "../api/tracks.ts";

export function useAlbumTracks(albumId?: string) {
    return useQuery({
        queryKey: ["albumTracks", albumId],
        queryFn: () => getAlbumTracks(albumId!),

        enabled: !!albumId,
    });
}
