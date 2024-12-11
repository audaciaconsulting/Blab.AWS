### Code ready for review
- ✔ Code compiles, all tests pass, no debug/console statements or commented out code left in
- ❌ Build/tests fail or commented out code/debug statements left in
- ❎ No code changes (e.g. just a documentation change)

### PBI/Bug tested against AC
- ✔ Tested locally and all AC pass
- ❌ Not tested
- ❎ Testing cannot be performed locally

### Unit tests added/updated
- ✔ New server-side logic mostly tested by unit tests
- ❌ New server-side logic tested by few/no unit tests
- ❎ No server-side logic updated

### Wiki documentation and/or ADR added/updated
- ✔ New behaviour is explained in writing
- ❌ No documentation added/updated
- ❎ Bug fixes, or behaviour not changed, or behaviour is self-evident to users & developers

### Code written to be performant at scale
- ✔ New code is expected to run quickly and efficiently in production
- ❌ Code is expected to be slow/inefficient
- ❎ Changes will not impact performance

### Added/updated logging
- ✔ Most/all changed code contains appropriate logging
- ❌ Large portions of changed code contain no logging
- ❎ No significant server-side changes

### Implemented security measures and followed secure coding standards
- ✔ Permissions or other security measures changed or added to new functionality
- ❌ Security not relevant to these changes
- ❎ Functionality covered by existing security measures and these have not changed

### Automated testing has been accounted for ([Swagger documentation added/updated](https://dev.azure.com/audacia/Audacia/_wiki/wikis/Audacia%20Technical%20Standards/6675/REST-API-Design-Guidelines?anchor=documenting-apis), UI selectors added)
- ✔ All controllers in the PR have attribute on all endpoints and UI selectors added/updated to all relevant elements
- ❌ Controller edited that has missing attribute(s) and/or some relevant elements do not have a UI selector
- ❎ No changes to controllers and no HTML changes