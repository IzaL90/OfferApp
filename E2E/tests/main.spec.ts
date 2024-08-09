import { test, expect } from '@playwright/test';

test('Smoke tests', async ({ page }) => {
    await page.goto('http://localhost:5050');
    await expect(page.locator("//table[@class='table table-striped']")).toBeVisible()
    await expect(page.locator("//h1[@data-name='offers-information']")).toHaveText("Hello, welcome to bid management!")
});