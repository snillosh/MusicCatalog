import {Button, Card, Center, Group, Image, Loader, Modal, Stack, Text, Title} from "@mantine/core";
import {useAlbumMetadataById} from "../hooks/useMetadata.ts";
import {useImportAlbum} from "../hooks/useImportAlbum.ts";

type AddAlbumModelProps = {
    importModalOpen: boolean;
    selectedReleaseGroupId: string;
    setImportModalOpen: (modelOpen: boolean) => void;
}

export function AddAlbumModel({importModalOpen, selectedReleaseGroupId, setImportModalOpen}: AddAlbumModelProps) {
    const {
        data: preview,
        isLoading,
        error,
    } = useAlbumMetadataById(selectedReleaseGroupId);

    const importAlbumMutation = useImportAlbum();

    return <Modal
        opened={importModalOpen}
        onClose={() => setImportModalOpen(false)}
        title="Import Album"
        size="xl"
    >
        {isLoading && <Loader/>}

        {error && <Text c="red">Failed to load album preview.</Text>}

        {preview && (
            <>
                <Stack>
                    <Group justify="space-evenly">
                        <Stack>
                            <Image
                                src={`https://coverartarchive.org/release/${preview.musicBrainzReleaseId}/front-250`}
                                alt={preview.title}
                                w={200}
                                h={200}
                                radius="sm"
                            />
                            <Title order={3}>{preview.title}</Title>
                            <Text c="dimmed">{preview.artistName}</Text>
                        </Stack>

                        <Card>
                            <Title order={4}>Tracklist</Title>

                            {preview.tracks.map(t => <Text>{t.trackNumber}. {t.title}</Text>)}
                        </Card>
                    </Group>

                    <Center m="lg">
                        <Button
                            loading={importAlbumMutation.isPending}
                            disabled={!selectedReleaseGroupId}
                            onClick={() => {
                                if (!selectedReleaseGroupId) {
                                    return;
                                }

                                importAlbumMutation.mutate(
                                    {
                                        releaseGroupId: selectedReleaseGroupId,
                                    },
                                    {
                                        onSuccess: () => {
                                            setImportModalOpen(false);
                                        },
                                    }
                                );
                            }}
                        >
                            Import album
                        </Button>
                    </Center>
                </Stack>
            </>
        )}
    </Modal>
}
