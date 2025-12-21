export function setMetaTags(title, description, imageUrl) {
    const titleTag = document.querySelector('meta[property="og:title"]');
    if (titleTag) titleTag.content = title;

    const descTag = document.querySelector('meta[property="og:description"]');
    if (descTag) descTag.content = description;

    const imgTag = document.querySelector('meta[property="og:image"]');
    if (imgTag) imgTag.content = imageUrl;
}
