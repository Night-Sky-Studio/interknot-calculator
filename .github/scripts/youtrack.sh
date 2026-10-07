#!/usr/bin/env bash
# Minimal YouTrack client for the Claude reviewer. It can only search and
# create issues in project IKC, so the reviewer never needs raw curl access.
#
#   youtrack.sh search "<YouTrack query>"
#   youtrack.sh create <Bug|Task|Feature|Improvement> <Low|Medium|High> "<summary>" "<description>"
#
# Requires YOUTRACK_TOKEN. PR_URL, if set, is appended to new issue descriptions.

set -euo pipefail

YT_URL="https://youtrack.interknot.space"
PROJECT_ID="0-3" # IKC
ASSIGNEE="lilystilson"

: "${YOUTRACK_TOKEN:?YOUTRACK_TOKEN is not set}"

api() {
  curl -sS --fail-with-body \
    -H "Authorization: Bearer ${YOUTRACK_TOKEN}" \
    -H "Accept: application/json" \
    "$@"
}

case "${1:-}" in
  search)
    [[ $# -eq 2 ]] || { echo "usage: $0 search \"<query>\"" >&2; exit 2; }
    api -G "${YT_URL}/api/issues" \
      --data-urlencode "query=project: IKC $2" \
      --data-urlencode 'fields=idReadable,summary,resolved' \
      --data-urlencode '$top=20' \
      | jq -r '.[] | "\(.idReadable)\t\(if .resolved then "resolved" else "open" end)\t\(.summary)"'
    ;;

  create)
    [[ $# -eq 5 ]] || { echo "usage: $0 create <type> <priority> \"<summary>\" \"<description>\"" >&2; exit 2; }
    type="$2" priority="$3" summary="$4" description="$5"
    [[ "$type" =~ ^(Bug|Task|Feature|Improvement)$ ]] || { echo "invalid type: $type" >&2; exit 2; }
    [[ "$priority" =~ ^(Low|Medium|High)$ ]] || { echo "invalid priority: $priority" >&2; exit 2; }
    if [[ -n "${PR_URL:-}" ]]; then
      description+=$'\n\n'"Source: ${PR_URL}"
    fi

    body=$(jq -n \
      --arg project "$PROJECT_ID" --arg summary "$summary" --arg description "$description" \
      --arg type "$type" --arg priority "$priority" --arg assignee "$ASSIGNEE" \
      '{
        project: { id: $project },
        summary: $summary,
        description: $description,
        customFields: [
          { name: "Type",     "$type": "SingleEnumIssueCustomField", value: { name: $type } },
          { name: "Priority", "$type": "SingleEnumIssueCustomField", value: { name: $priority } },
          { name: "Assignee", "$type": "SingleUserIssueCustomField", value: { login: $assignee } }
        ]
      }')

    id=$(api -X POST "${YT_URL}/api/issues?fields=idReadable" \
      -H "Content-Type: application/json" -d "$body" | jq -r '.idReadable')
    echo "${YT_URL}/issue/${id}"
    ;;

  *)
    echo "usage: $0 {search|create} ..." >&2
    exit 2
    ;;
esac
