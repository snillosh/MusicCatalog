import { useQuery } from "@tanstack/react-query";
import { getAlbums } from "../api/albums";

export function useAlbums() {
    return useQuery({
        queryKey: ["albums"],
        queryFn: getAlbums,
    });
}
