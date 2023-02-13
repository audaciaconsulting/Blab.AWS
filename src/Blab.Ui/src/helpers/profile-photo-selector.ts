import defaultImage from "@/assets/default-profile-picture.jpg";
export function profilePhotoSelector(
  blobName: string | null | undefined
): string {
  if (blobName?.split("/").pop()) {
    return blobName;
  }
  return defaultImage;
}
