$TYPE = "DEV"
$LAST_TAG = git describe --tags --abbrev=0
$COMMIT_COUNT = git rev-list "$LAST_TAG..HEAD" --count
$DATE = Get-Date -Format "yyyy.ddMM"
$CHANGE_LOG = git log "$LAST_TAG..HEAD" --pretty=format:"%h %s"

Write-Output "Create Tag $TYPE-$DATE.$COMMIT_COUNT"
Write-Output "Changelog:"
Write-Output $CHANGE_LOG

git tag "$TYPE-$DATE.$COMMIT_COUNT"