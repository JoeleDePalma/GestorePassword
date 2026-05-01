import { InformativeSectionActions } from "./actions.js";
import { InformativeSectionTypes } from "./actions.js";

// Attaches a click listener to each section button to update the displayed content
document.querySelectorAll(".sectionsDiv__sectionButton").forEach(btn => {
    btn.addEventListener("click", () => {
        const section = btn.getAttribute("data-section");
        if (!section) return;

        InformativeSectionActions.ChangeMainContent(
            section as InformativeSectionTypes.ContentKey
        );
    });
});
