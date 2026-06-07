import { api } from "./client";
import type { components } from "./schema";

export type TrackDto = components["schemas"]["TrackDto"];

export async function getAlbumTracks(albumId: string): Promise<TrackDto[]> {
    const { data, error } = await api.GET("/api/albums/{albumId}/tracks", {
        params: {
            path: {
                albumId,
            },
        },
    });

    if (error) {
        throw error;
    }

    return data ?? [];
}
