import {useMutation, useQueryClient} from "@tanstack/react-query";
import {importAlbum} from "../api/importing";

export function useImportAlbum() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: importAlbum,
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ["albums"],
            });
        },
    });
}
