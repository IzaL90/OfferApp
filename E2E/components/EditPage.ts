import { expect } from "@playwright/test";
import { Page, Locator } from "playwright";
import { FormComponent } from "./FormComponent";
import { AddComponent } from "./AddComponent";
import { Bid, editBid } from "../interfaces/Bid";

export class EditComponent {
    public readonly page: Page
    public readonly root: Locator;
    public readonly submit: Locator;
    public readonly cancel: Locator;
    public readonly form: FormComponent;
    public readonly modal: AddComponent;

    constructor(page: Page) {
        this.page = page
        this.submit = this.page.locator("//button[@data-name='bid-submit-button']");
        this.cancel = this.page.locator("//button[@data-name='bid-close-button']");
        this.form = new FormComponent(this.page);
    }

    async isVisible(): Promise<boolean> {
        return this.root.isVisible();
    }

    public async expectFormVisible(): Promise<void> {
        expect(this.form.expectFormVisible());
        await expect(this.submit).toBeVisible();
        await expect(this.cancel).toBeVisible();
    }

    public async editForm(values: Bid = editBid): Promise<void> {
        await this.form.name.fill(values.name);
        await this.form.price.fill(values.price);
        await this.form.description.fill(values.description);
    }
    public async clickSubmit(): Promise<void> {
        await this.submit.click();
    }
}