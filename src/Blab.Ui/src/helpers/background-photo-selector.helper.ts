import defaultImage from "@/assets/default-profile-background-picture.jpg";
export function backgroundPhotoSelector(
  blobName: string | null | undefined
): string {
  if (blobName?.split("/").pop()) {
    return blobName;
  }

  return defaultImage;
}
