using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionDirection {Left, Straight, Right}
public class PathGenerator : MonoBehaviour
{
    [SerializeField]int pathLeght = 10;
    [SerializeField]List<PathSection> pathSectionPrefabs;
    List<PathSection> availablePathSections = new();
    PathSection currentSection;
    int pathTilt = 0;

    List<PathSection> leftSections;
    List<PathSection> rightSections;
    void Start()
    {
        // leftSections.AddRange(pathSectionPrefabs.FindAll(section => section.GetDirection() == TransitionDirection.Left));
        // rightSections.AddRange(pathSectionPrefabs.FindAll(section => section.GetDirection() == TransitionDirection.Left));
        availablePathSections.AddRange(pathSectionPrefabs);
        ComposePath();
    }
    void ComposePath()
    {
        for(int i = 0;i < pathLeght; i++)
        {
            GenerateSection();
        }
    }
    void GenerateSection()
    {
        CalculateAvailablePaths();

        PathSection nextSection = Instantiate(availablePathSections[Random.Range(0,availablePathSections.Count)]);
        if(currentSection == null)
        {
            nextSection.transform.eulerAngles = new Vector3(0,90,0);
            currentSection = nextSection;
        }
            
        else
        {
            switch (currentSection.GetDirection())
            {
                case TransitionDirection.Left:
                    TransformSection(-60,currentSection.transform.position,currentSection.transform.rotation,nextSection);
                    pathTilt --;
                    break;
                case TransitionDirection.Straight:
                    TransformSection(0,currentSection.transform.position,currentSection.transform.rotation,nextSection);
                    pathTilt = 0;
                    break;
                case TransitionDirection.Right:
                    TransformSection(60,currentSection.transform.position,currentSection.transform.rotation,nextSection);
                    pathTilt ++;
                    break;
            }
        }
    }
    void TransformSection(int angle,Vector3 posCurSec, Quaternion rotCurSec, PathSection nextSection)
    {
        Quaternion rotation = Quaternion.Euler(rotCurSec.eulerAngles + new Vector3(0, angle, 0));
        nextSection.transform.rotation = rotation;

        if(angle < 0 )
        {
            nextSection.transform.position = posCurSec;
            nextSection.transform.position += new Vector3(-1.5f,0,0.866025404f);
        }

        else if(angle == 0)
        {
            nextSection.transform.position = posCurSec;
            nextSection.transform.position += new Vector3(0,0,0.866025404f*2);
        }

        else if(angle > 0)
        {
            nextSection.transform.position = posCurSec;
            nextSection.transform.position += new Vector3(1.5f,0,0.866025404f);
        }
        currentSection = nextSection;
    }

    void CalculateAvailablePaths()
    {
        switch (pathTilt)
        {
            case -1:
                availablePathSections.RemoveAll(sections => sections.GetDirection() == TransitionDirection.Left);
                break;

            case 0:
                availablePathSections = pathSectionPrefabs;
                break;

            case 1:
                availablePathSections.RemoveAll(sections => sections.GetDirection() == TransitionDirection.Right);
                break;

            default:
            Debug.LogError($"pathTilt out of boundaries: {pathTilt}");
                break;
        }
    }
}

