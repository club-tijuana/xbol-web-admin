export function postForm(action, fields) {
    const form = document.createElement("form");
    form.method = "post";
    form.action = action;
    form.style.display = "none";

    for (const [name, value] of Object.entries(fields ?? {})) {
        if (value === null || value === undefined)
            continue;

        const input = document.createElement("input");
        input.type = "hidden";
        input.name = name;
        input.value = value;
        form.appendChild(input);
    }

    document.body.appendChild(form);
    form.submit();
}
